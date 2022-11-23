using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;
using System.Linq;
using System.Diagnostics;
using TicTacToe;
using System.Runtime.CompilerServices;

namespace TicTacToe.AI
{
    internal class ClassicAI_Hashing : ClassicAI {
        public int HashHits { get; private set; }
        public int AllHits { get; private set; }
        public int HashCount { get { return _outcomes.Count; } }

        private ClassicBoardHasher _hasher = new ClassicBoardHasher();
        private Dictionary<string,int> _outcomes = new Dictionary<string,int>(0);

        #region BaseRequiredFunctions
        public override ClassicMoveEval GetBestMove(IClassicBoard board, int player) {
            return GetBestMoveNoRecVal(board, player);
        }

        public override List<ClassicMoveEval> GetAllBestMoves(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);

            var allBestMoves = new List<ClassicMoveEval>(9);
            var bestMoveSoFar = new ClassicMoveEval(-1, player, -2);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = player;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(player), out int res)) {
                    res = -1 * BestResultNoRecVal(AnotherPlayer(player), -1 * bestMoveSoFar.PlayerOutcome + 1);
                    UpdateAllHashes(player, res);
                } else {
                    HashHits++;
                }

                if (res >= bestMoveSoFar.PlayerOutcome) {
                    if (res > bestMoveSoFar.PlayerOutcome) {
                        allBestMoves.Clear();
                    }
                    bestMoveSoFar = new ClassicMoveEval(idx, player, res);
                    allBestMoves.Add(bestMoveSoFar);
                }
                _board[idx] = 0;
            }
            return allBestMoves;
        }

        public override List<ClassicMoveEval> GetAllMoves(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);

            var allMoves = new List<ClassicMoveEval>(9);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = player;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(player), out int res)) {
                    res = -1 * BestResultNoRecVal(AnotherPlayer(player),  BestPossibleMoveValue);
                    UpdateAllHashes(player, res);
                } else {
                    HashHits++;
                }

                allMoves.Add(new ClassicMoveEval(idx, player, res));

                _board[idx] = 0;
            }

            return allMoves;
        }
        #endregion

        #region GetBestMoveRecursionVal
        internal override ClassicMoveEval  GetBestMoveRecVal(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);

            var bestMoveSoFar = new ClassicMoveEval (-1, player, -2);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = player;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(player), out int res)) {
                    res = -1 * BestResultRecVal(AnotherPlayer(player), -1 * bestMoveSoFar.PlayerOutcome);
                    UpdateAllHashes(player, res);
                } else {
                    HashHits++;
                }

                if (res > bestMoveSoFar.PlayerOutcome)
                    bestMoveSoFar = new ClassicMoveEval (idx, player, res);

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
                    res = -1 * BestResultRecVal(AnotherPlayer(player), -1 * bestResultSoFar);
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
        internal override ClassicMoveEval  GetBestMoveNoRecVal(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);

            var bestMoveSoFar = new ClassicMoveEval (-1, player, -2);
            foreach (var cell in GetEmptyCells()) {
                _board[cell] = player;

                AllHits++;
                if (!_outcomes.TryGetValue(_hasher.GetHash(player), out int res)) {
                    res = -1 * BestResultNoRecVal(AnotherPlayer(player), -1 * bestMoveSoFar.PlayerOutcome);
                } else {
                    HashHits++;
                }

                if (res > bestMoveSoFar.PlayerOutcome) {
                    bestMoveSoFar = new ClassicMoveEval (cell,player, res);
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
                if (idx > 8) {
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
                    currentPlayer = AnotherPlayer(currentPlayer);

                    if (values[depth] >= uBounds[depth]) {   //Don't check another moves, alredy found the best
                        idx = 9;
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
                        idx = 9;

                    } else {//Outcome still undetermined, calculate outcome after another player moves
                        currentPlayer = AnotherPlayer(currentPlayer);
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
            foreach(var hash in _hasher.GetAllEquivalentHashes(player)) {
                _outcomes[hash] = outcome;
            }
        }

        protected override void ValidateAndPrepareAI(IClassicBoard board) {
            base.ValidateAndPrepareAI(board);

            _hasher.ChangeGrid(_board);
            _outcomes = new Dictionary<string, int>(10000);

            HashHits = 0;
            AllHits = 0;
        }
        #endregion


    }
}
