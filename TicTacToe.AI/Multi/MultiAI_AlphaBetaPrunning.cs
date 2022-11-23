using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;
using System.Linq;
using System.Diagnostics;

namespace TicTacToe.AI
{
    internal class MultiAI_AlphaBetaPrunning : MultiAI
    {


        #region BaseRequiredFunctions
        public override MultiMoveEval GetBestMove(IMultiGame game)
        {
            return GetBestMoveNoRecVal(game);
        }

        public override List<MultiMoveEval> GetAllBestMoves(IMultiGame game)
        {
            ValidateAndPrepareAI(game);

            var allBestMoves = new List<MultiMoveEval>();
            var bestMoveSoFar = new MultiMoveEval(game.Board.Space, 0, game.NextPlayer, -2);
            foreach (var idx in GetEmptyCells())
            {
                _board[idx] = game.NextPlayer;
                var res = -1 * BestResultNoRecVal(NextPlayer(game.NextPlayer), -1 * bestMoveSoFar.PlayerOutcome + 1);
                if (res >= bestMoveSoFar.PlayerOutcome)
                {
                    if (res > bestMoveSoFar.PlayerOutcome)
                    {
                        allBestMoves.Clear();
                    }
                    bestMoveSoFar = new MultiMoveEval(game.Board.Space, idx, game.NextPlayer, res);
                    allBestMoves.Add(bestMoveSoFar);
                }
                _board[idx] = 0;
            }
            return allBestMoves;
        }

        public override List<MultiMoveEval> GetAllMoves(IMultiGame game)
        {
            ValidateAndPrepareAI(game);

            var allMoves = new List<MultiMoveEval>();
            foreach (var idx in GetEmptyCells())
            {
                _board[idx] = game.NextPlayer;
                allMoves.Add(new MultiMoveEval(game.Board.Space, idx, game.NextPlayer, -1 * BestResultNoRecVal(NextPlayer(game.NextPlayer), BestPossibleMoveValue)));
                _board[idx] = 0;
            }

            return allMoves;
        }
        #endregion


        #region GetBestMoveRecursionVal
        internal override MultiMoveEval GetBestMoveRecVal(IMultiGame game)
        {
            ValidateAndPrepareAI(game);

            var bestMoveSoFar = new MultiMoveEval(game.Board.Space, 0, game.NextPlayer, -2);
            foreach (var idx in GetEmptyCells())
            {
                _board[idx] = game.NextPlayer;

                var res = -1 * BestResultRecVal(NextPlayer(game.NextPlayer), -1 * bestMoveSoFar.PlayerOutcome,0);
                if (res > bestMoveSoFar.PlayerOutcome)
                    bestMoveSoFar = new MultiMoveEval(game.Board.Space, idx, game.NextPlayer, res);

                _board[idx] = 0;

                if (bestMoveSoFar.PlayerOutcome >= BestPossibleMoveValue)
                    return bestMoveSoFar;
            }
            return bestMoveSoFar;
        }

        private int BestResultRecVal(int player, int upperBound,int depth)
        {
            if (upperBound > BestPossibleMoveValue)
                upperBound = BestPossibleMoveValue;

            var outcome = GetOutcomeForPlayer(player);
            if (outcome.HasValue)
                return outcome.Value;

           if (depth >= MaxDepth)
                return TryEvaulatingBoard(player);


                var bestResultSoFar = -2;
            foreach (var cell in GetEmptyCells())
            {
                _board[cell] = player;

                var res = -1 * BestResultRecVal(NextPlayer(player), -1 * bestResultSoFar, depth+1);
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
        internal override MultiMoveEval GetBestMoveNoRecVal(IMultiGame game)
        {
            ValidateAndPrepareAI(game);

            var bestMoveSoFar = new MultiMoveEval(game.Board.Space, 0, game.NextPlayer, -2);
            foreach (var cell in GetEmptyCells())
            {
                _board[cell] = game.NextPlayer;
                var res = -1 * BestResultNoRecVal(NextPlayer(game.NextPlayer), -1 * bestMoveSoFar.PlayerOutcome);
                if (res > bestMoveSoFar.PlayerOutcome)
                {
                    bestMoveSoFar = new MultiMoveEval(game.Board.Space, cell, game.NextPlayer, res);
                }
                _board[cell] = 0;

                if (bestMoveSoFar.PlayerOutcome >= BestPossibleMoveValue)
                {
                    return bestMoveSoFar;
                }
            }
            return bestMoveSoFar;
        }

        private int BestResultNoRecVal(int player, int upperBound)
        {
            var outcome = GetOutcomeForPlayer(player);
            if (outcome.HasValue)
                return outcome.Value;


            int depth = 0, idx = 0;
            int currentPlayer = player;
            int[] values = new int[_board.Length];
            int[] lastMoveIdx = new int[_board.Length];
            int[] uBounds = new int[_board.Length];

            values.Fill(-2);
            lastMoveIdx.Fill(-1);
            uBounds.Fill(BestPossibleMoveValue);
            uBounds[0] = upperBound;

            while (true)
            {
                if (idx >= _board.Length)
                {
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

                    if (values[depth] >= uBounds[depth])
                    {   //Don't check another moves, alredy found the best
                        idx = _board.Length;
                    }
                    else
                    { //Clear the current move and check another moves that left (if any)
                        _board[lastMoveIdx[depth]] = 0;
                        idx = lastMoveIdx[depth] + 1;
                    }

                }
                else if (_board[idx] == 0)
                {  //Make move
                    _board[idx] = currentPlayer;
                    lastMoveIdx[depth] = idx;

                    outcome = GetOutcomeForPlayer(currentPlayer);
                    if (outcome.HasValue)
                    {//Outcome can be determined, so game is finished - no point in checking another moves at this depth
                        if (outcome.Value > values[depth])
                            values[depth] = outcome.Value;
                        idx = _board.Length;


                    }
                    else if (depth >= MaxDepth)
                    {//Try evaulating board
                        outcome = TryEvaulatingBoard(currentPlayer);
                        if (outcome.Value > values[depth])
                            values[depth] = outcome.Value;
                        idx = _board.Length;

                    }

                    else
                    {//Outcome still undetermined, calculate outcome after another player moves

                        currentPlayer = NextPlayer(currentPlayer);
                        idx = 0;
                        depth++;
                        uBounds[depth] = -1 * values[depth - 1];

                    }

                }
                else
                {    //Look for next empty cell
                    idx++;
                }
                // Debug.Print($"idx={idx}; depth={depth}");
            }
        }

        private int TryEvaulatingBoard(int player)
        {
            //get wins with lwoest number of moves required
            var playersPossibilities = _boardValidator.PossibleWins.Where(x => x.Select(y => _board[y]).Except(new int[] { 0, player }).Count() == 0);
            //get wins possible for other player
            var otherPlayersPosibilities = _boardValidator.PossibleWins.Where(x => x.Select(y => _board[y]).Except(new int[] { 0, player }).Count() == 1);


            var minTurnsForPlayerTofinish = playersPossibilities.Count() == 0 ? 0 : playersPossibilities.Min(x => x.Select(y => _board[y]).Except(new int[] { 0 }).Count());
            var minTurnsForOpponentsToFinish = otherPlayersPosibilities.Count() == 0 ? 0 : otherPlayersPosibilities.Min(x => x.Select(y => _board[y]).Except(new int[] { 0 }).Count());
            if (minTurnsForPlayerTofinish >= minTurnsForOpponentsToFinish)
            {
                return 1;
            }
            else if (minTurnsForPlayerTofinish == minTurnsForOpponentsToFinish - 1)
            {
                return 0;
            }
            else
            {
                return -1;
            }


        }
        #endregion
    }
}
