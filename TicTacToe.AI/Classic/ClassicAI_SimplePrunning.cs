using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;
using System.Linq;
using System.Diagnostics;

namespace TicTacToe.AI
{
    internal class ClassicAI_SimplePrunning : ClassicAI {

        #region BaseRequiredFunctions
        public override ClassicMoveEval GetBestMove(IClassicBoard board, int player) {
            return GetBestMoveNoRecVal(board,player);
        }

        public override List<ClassicMoveEval> GetAllBestMoves(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);

            var allBestMoves = new List<ClassicMoveEval>(9);
            var bestMoveSoFar = new ClassicMoveEval(-1, player, int.MinValue);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = player;
                var res = -1 * BestResultNoRecVal(AnotherPlayer(player));
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
                allMoves.Add(new ClassicMoveEval(idx, player, -1 * BestResultNoRecVal(AnotherPlayer(player))));
                _board[idx] = 0;
            }

            return allMoves;
        }
        #endregion

        //Subfunction returns int (which forces loop repeat in main function)
        #region GetBestMoveRecursionVal
        internal override ClassicMoveEval GetBestMoveRecVal(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);

            var bestMoveSoFar = new ClassicMoveEval(-1, player, int.MinValue);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = player;

                var res = -1 * BestResultRecVal(AnotherPlayer(player));
                if (res > bestMoveSoFar.PlayerOutcome)
                    bestMoveSoFar = new ClassicMoveEval(idx, player, res);

                _board[idx] = 0;

                if (bestMoveSoFar.PlayerOutcome >= BestPossibleMoveValue)
                    return bestMoveSoFar;
            }
            return bestMoveSoFar;
        }

        private int BestResultRecVal(int player) {
            var outcome = GetOutcomeForPlayer(player);
            if (outcome.HasValue)
                return outcome.Value;

            var bestResultSoFar = int.MinValue;
            foreach (var cell in GetEmptyCells()) {
                _board[cell] = player;

                var res = -1 * BestResultRecVal(AnotherPlayer(player));
                if (res > bestResultSoFar)
                    bestResultSoFar = res;

                _board[cell] = 0;

                if (bestResultSoFar >= BestPossibleMoveValue)
                    return bestResultSoFar;
            }
            return bestResultSoFar;
        }
        #endregion

        //Subfunction returns struct (no loop repeat in main function)(slightly slower)
        #region GetBestMoveRecursionStruct
        public ClassicMoveEval GetBestMoveRecStruct(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);
            return BestMoveRecStruct(player);
        }

        private ClassicMoveEval BestMoveRecStruct(int player) {
            var bestMove = new ClassicMoveEval(-1, player, int.MinValue);
            foreach (var cell in GetEmptyCells()) {
                _board[cell] = player;

                var outcome = GetOutcomeForPlayer(player);
                if (!outcome.HasValue)
                    outcome = -1 * BestMoveRecStruct(AnotherPlayer(player)).PlayerOutcome;

                if (outcome.Value > bestMove.PlayerOutcome)
                    bestMove = new ClassicMoveEval(cell, player, outcome.Value);

                _board[cell] = 0;

                if (bestMove.PlayerOutcome >= BestPossibleMoveValue)
                    return bestMove;
            }
            return bestMove;
        }
        #endregion

        //Similar in terms of speed
        #region GetBestMoveNoRecursionVal
        internal override ClassicMoveEval GetBestMoveNoRecVal(IClassicBoard board, int player) {
            ValidateAndPrepareAI(board);

            var bestMoveSoFar = new ClassicMoveEval(-1, player, int.MinValue);
            foreach (var idx in GetEmptyCells()) {
                _board[idx] = player;
                var res = -1 * BestResultNoRecVal(AnotherPlayer(player));
                if (res > bestMoveSoFar.PlayerOutcome)
                    bestMoveSoFar = new ClassicMoveEval(idx, player, res);

                _board[idx] = 0;

                if (bestMoveSoFar.PlayerOutcome >= BestPossibleMoveValue)
                    return bestMoveSoFar;
            }
            return bestMoveSoFar;
        }

        //Old one, more ugly and doesn't work correctly
        private int BestResultNoRec(int player) {
            var movesHistory = new Stack<int>();
            var outcomesHistory = new Stack<int>();
            int idx = 0;
            var currentPlayer = player;

            var outcome = GetOutcomeForPlayer(player);
            if (outcome.HasValue) {
                return outcome.Value;
            }


            while (idx >= 0) {
                if (idx > 8) {
                    if (movesHistory.Count == 0) {
                        return outcomesHistory.Pop();
                    }
                    if (outcomesHistory.Count > 1) {
                        //Outcome determined, update previous one 
                        var currentVal = outcomesHistory.Pop();
                        var prevVal = outcomesHistory.Pop();
                        outcomesHistory.Push(currentVal > prevVal ? currentVal : prevVal);
                    }

                    if (outcomesHistory.Count > 1) {
                        //Outcome determined, update previous one 
                        var currentVal = -1 * outcomesHistory.Pop();
                        var prevVal = outcomesHistory.Pop();
                        outcomesHistory.Push(currentVal > prevVal ? currentVal : prevVal);
                    }
                    if (outcomesHistory.Count > 1) {
                        //Outcome determined, update previous one 
                        var currentVal = outcomesHistory.Pop();
                        var prevVal = outcomesHistory.Pop();
                        outcomesHistory.Push(currentVal > prevVal ? currentVal : prevVal);
                    }

                    //start completing cells from next one
                    idx = movesHistory.Pop();
                    _board[idx] = 0;
                    idx++;
                    if (idx > 9) {
                        outcomesHistory.Push(int.MinValue);
                    }
                    currentPlayer = AnotherPlayer(currentPlayer);

                } else if (_board[idx] == 0) {
                    var firstTime = true;
                    for (int i = 0; i < idx; i++) {
                        if (_board[i] == 0) {
                            firstTime = false;
                            break;
                        }
                    }
                    if (firstTime) {
                        outcomesHistory.Push(int.MinValue);
                    }

                    //place player
                    _board[idx] = currentPlayer;
                    movesHistory.Push(idx);

                    //Add Outcome if can be determined
                    outcome = GetOutcomeForPlayer(player);
                    if (outcome.HasValue) {
                        return outcome.Value;
                    }

                    if (!outcome.HasValue) {
                        //Can't determine outcome, addd minimum
                        outcomesHistory.Push(int.MinValue);
                        //Start looking for any empty cell
                        idx = 0;
                        currentPlayer = AnotherPlayer(currentPlayer);
                    } else {
                        _board[movesHistory.Pop()] = 0;
                        outcomesHistory.Push(outcome.Value);
                        idx = 9;
                        //currentPlayer = AnotherPlayer(currentPlayer);
                    }
                } else {
                    idx++;
                }


            }
            return 0;

        }

        private int BestResultNoRecVal(int player) {
            var outcome = GetOutcomeForPlayer(player);
            if (outcome.HasValue)
                return outcome.Value;

            int depth = 0, idx = 0;
            int currentPlayer = player;
            int[] values = new int[_board.Length];
            int[] lastMoveIdx = new int[_board.Length];

            values.Fill(int.MinValue);
            lastMoveIdx.Fill(-1);

            while (true) {
                if (idx > 8) {
                    _board[lastMoveIdx[depth]] = 0;

                    if (depth == 0)
                        return values[depth];

                    //Update previous player outcome based on current
                    if (-1 * values[depth] > values[depth - 1])
                        values[depth - 1] = -1 * values[depth];

                    //Go back to previous player, clear current value
                    values[depth] = int.MinValue;
                    depth--;
                    currentPlayer = AnotherPlayer(currentPlayer);

                    if (values[depth] >= BestPossibleMoveValue) {   //Don't check another moves, alredy found the best
                        idx = 9;
                    } else { //Clear the current move and check another moves that left (if any)
                        _board[lastMoveIdx[depth]] = 0;
                        idx = lastMoveIdx[depth] + 1;
                    }

                } else if (_board[idx] == 0) {  //Make move
                    _board[idx] = currentPlayer;
                    lastMoveIdx[depth] = idx;

                    outcome = GetOutcomeForPlayer(currentPlayer);
                    if (outcome.HasValue) {//Outcome can be determined, so game is finished - no point in checking another moves at this depth
                        if (outcome.Value > values[depth])
                            values[depth] = outcome.Value;
                        idx = 9;

                    } else {//Outcome still undetermined, calculate outcome after another player moves
                        currentPlayer = AnotherPlayer(currentPlayer);
                        idx = 0;
                        depth++;
                    }

                } else {    //Look for next empty cell
                    idx++;
                }
                // Debug.Print($"idx={idx}; depth={depth}");
            }
        }
        #endregion
    }
}
