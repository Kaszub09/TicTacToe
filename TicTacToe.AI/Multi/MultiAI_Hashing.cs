using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;
using System.Linq;
using System.Diagnostics;
using TicTacToe.AI;
using System.Runtime.CompilerServices;

namespace TicTacToe.AI
{
    internal class MultiAI_Hashing : MultiAI {
        public int HashHits { get; private set; }
        public int AllHits { get; private set; }
        public int HashCount { get { return _outcomes.Count; } }

        private MultiBoardHasher _hasher = new MultiBoardHasher();
        private Dictionary<string,int> _outcomes = new Dictionary<string,int>();

        #region BaseRequiredFunctions
        public override MultiMoveEval GetBestMove(IMultiGame game) {
            return GetBestMoveNoRecVal(game);
        }

        public override List<MultiMoveEval> GetAllBestMoves(IMultiGame game) {
            ValidateAndPrepareAI(game);

            var allBestMoves = new List<MultiMoveEval>();
            var bestMoveSoFar = new MultiMoveEval(game.Board.Space, 0, game.NextPlayer, -2);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = game.NextPlayer;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(game.NextPlayer), out int res)) {
                    res = -1 * BestResultNoRecVal(NextPlayer(game.NextPlayer), -1 * bestMoveSoFar.PlayerOutcome + 1);
                    UpdateAllHashes(game.NextPlayer, res);
                } else {
                    HashHits++;
                }

                if (res >= bestMoveSoFar.PlayerOutcome) {
                    if (res > bestMoveSoFar.PlayerOutcome) {
                        allBestMoves.Clear();
                    }
                    bestMoveSoFar = new MultiMoveEval(game.Board.Space, idx, game.NextPlayer, res);
                    allBestMoves.Add(bestMoveSoFar);
                }
                _board[idx] = 0;
            }
            return allBestMoves;
        }

        public override List<MultiMoveEval> GetAllMoves(IMultiGame game) {
            ValidateAndPrepareAI(game);

            var allMoves = new List<MultiMoveEval>();
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = game.NextPlayer;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(game.NextPlayer), out int res)) {
                    res = -1 * BestResultNoRecVal(NextPlayer(game.NextPlayer),  BestPossibleMoveValue);
                    UpdateAllHashes(game.NextPlayer, res);
                } else {
                    HashHits++;
                }

                allMoves.Add(new MultiMoveEval(game.Board.Space, idx, game.NextPlayer, res));

                _board[idx] = 0;
            }

            return allMoves;
        }
        #endregion

        #region GetBestMoveRecursionVal
        internal override MultiMoveEval GetBestMoveRecVal(IMultiGame game) {
            ValidateAndPrepareAI(game);

            var bestMoveSoFar = new MultiMoveEval(game.Board.Space, 0, game.NextPlayer, -2);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = game.NextPlayer;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(game.NextPlayer), out int res)) {
                    res = -1 * BestResultRecVal(NextPlayer(game.NextPlayer), -1 * bestMoveSoFar.PlayerOutcome);
                    UpdateAllHashes(game.NextPlayer, res);
                } else {
                    HashHits++;
                }

                if (res > bestMoveSoFar.PlayerOutcome)
                    bestMoveSoFar = new MultiMoveEval(game.Board.Space, idx, game.NextPlayer, res);

                _board[idx] = 0;

                if (bestMoveSoFar.PlayerOutcome >= BestPossibleMoveValue)
                    return bestMoveSoFar;
            }
            return bestMoveSoFar;
        }

        private int BestResultRecVal(int player, int upperBound) {
            if (upperBound > BestPossibleMoveValue)
                upperBound = BestPossibleMoveValue;

            var outcome = GetOutcomeForPlayer(player);
            if (outcome.HasValue) {
                //If we got there, it means hash wasn't in _outcomes just before calling this functions, so no need to check
                UpdateAllHashes(player, outcome.Value);
                return outcome.Value;
            }

            var bestResultSoFar = -2;
            foreach (var cell in GetEmptyCells()) {
                _board[cell] = player;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(player), out int res)) {
                    res = -1 * BestResultRecVal(NextPlayer(player), -1 * bestResultSoFar);
                } else {
                    HashHits++;
                }

                if (res > bestResultSoFar)
                    bestResultSoFar = res;

                _board[cell] = 0;

                if (bestResultSoFar >= upperBound)
                    return bestResultSoFar;
            }
            return bestResultSoFar;
        }
        #endregion

        #region GetBestMoveNoRecursionVal
        internal override MultiMoveEval GetBestMoveNoRecVal(IMultiGame game) {
            ValidateAndPrepareAI(game);

            var bestMoveSoFar = new MultiMoveEval(game.Board.Space, 0, game.NextPlayer, -2);
            foreach (var cell in GetEmptyCells()) {
                _board[cell] = game.NextPlayer;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(game.NextPlayer), out int res)) {
                    res = -1 * BestResultNoRecVal(NextPlayer(game.NextPlayer), -1 * bestMoveSoFar.PlayerOutcome);
                } else {
                    HashHits++;
                }

                if (res > bestMoveSoFar.PlayerOutcome) {
                    bestMoveSoFar = new MultiMoveEval(game.Board.Space, cell, game.NextPlayer, res);
                }
                _board[cell] = 0;

                if (bestMoveSoFar.PlayerOutcome >= BestPossibleMoveValue) {
                    return bestMoveSoFar;
                }
            }
            return bestMoveSoFar;
        }

        private int BestResultNoRecVal(int player, int upperBound) {
            var outcome = GetOutcomeForPlayer(player);
            if (outcome.HasValue) {
                //If we got there, it means hash wasn't in _outcomes just before calling this functions, so no need to check
                UpdateAllHashes(player, outcome.Value);
                return outcome.Value;
            }

            int depth = 0, idx = 0;
            int currentPlayer = player;
            int[] values = new int[_board.Length];
            int[] lastMoveIdx = new int[_board.Length];
            int[] uBounds = new int[_board.Length];

            values.Fill(-2);
            lastMoveIdx.Fill(-1);
            uBounds.Fill(BestPossibleMoveValue);
            uBounds[0] = upperBound;

            while (true) {
                if (idx >= _board.Length) {
                    _board[lastMoveIdx[depth]] = 0;
                    uBounds[depth] = BestPossibleMoveValue;

                    if (depth == 0)
                        return values[depth];

                    //Update previous player outcome based on current
                    if (-1 * values[depth] > values[depth - 1])
                        values[depth - 1] = -1 * values[depth];

                    //Go back to previous player, clear current value
                    values[depth] = -2;
                    depth--;
                    currentPlayer = PreviousPlayer(currentPlayer);

                    if (values[depth] >= uBounds[depth]) {   //Don't check another moves, alredy found the best
                        idx = _board.Length;
                    } else { //Clear the current move and check another moves that left (if any)
                        _board[lastMoveIdx[depth]] = 0;
                        idx = lastMoveIdx[depth] + 1;
                    }

                } else if (_board[idx] == 0) {  //Make move
                    _board[idx] = currentPlayer;
                    lastMoveIdx[depth] = idx;

                    AllHits++;
                    if (_outcomes.TryGetValue(_hasher.GetHash(currentPlayer), out int res)) {
                        HashHits++;
                        outcome = res;
                    } else {
                        outcome = GetOutcomeForPlayer(currentPlayer);
                        if (outcome.HasValue) 
                            UpdateAllHashes(currentPlayer, outcome.Value);
                    }

                    if (outcome.HasValue) {//Outcome can be determined, so game is finished - no point in checking another moves at this depth
                        if (outcome.Value > values[depth])
                            values[depth] = outcome.Value;
                        idx = _board.Length;

                    } else {//Outcome still undetermined, calculate outcome after another player moves
                        currentPlayer = NextPlayer(currentPlayer);
                        idx = 0;
                        depth++;
                        uBounds[depth] = -1 * values[depth - 1];
                    }

                } else {    //Look for next empty cell
                    idx++;
                }
            }
        }

        #endregion

        #region HelpersFunctions
        private void UpdateAllHashes(int player, int outcome) {
            foreach (var hash in _hasher.GetAllEquivalentHashes(player))
            {
                _outcomes[hash] = outcome;
            }
        }

        protected override void ValidateAndPrepareAI(IMultiGame game) {
            base.ValidateAndPrepareAI(game);

            _hasher.ChangeGrid(game.Board.Space,_board);
            _outcomes = new Dictionary<string, int>(100000);

            HashHits = 0;
            AllHits = 0;
        }
        #endregion


    }
}
