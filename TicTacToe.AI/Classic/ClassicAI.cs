using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.AI {
    /// <summary>
    /// Base of classes for finding best move for a player
    /// </summary>
    public abstract class ClassicAI {
        protected const int BestPossibleMoveValue = 1;
        protected int[] _board;
        internal ClassicBoardValidator _boardValidator;

        internal ClassicAI() {
            _boardValidator = new ClassicBoardValidator();
        }

        #region GetMoves
        public ClassicMoveEval GetBestMove(IClassicGame game)  =>  GetBestMove(game.Board, game.NextPlayer);
        public List<ClassicMoveEval> GetAllBestMoves(IClassicGame game) => GetAllBestMoves(game.Board, game.NextPlayer);
        public List<ClassicMoveEval> GetAllMoves(IClassicGame game)=> GetAllMoves(game.Board, game.NextPlayer);

        public abstract ClassicMoveEval GetBestMove(IClassicBoard board, int player);
        public abstract List<ClassicMoveEval> GetAllBestMoves(IClassicBoard board, int player);
        public abstract List<ClassicMoveEval> GetAllMoves(IClassicBoard board, int player);

        internal abstract ClassicMoveEval GetBestMoveRecVal(IClassicBoard board, int player);
        internal abstract ClassicMoveEval GetBestMoveNoRecVal(IClassicBoard board, int player);
        #endregion

        #region HelpersFunctions
        protected virtual void ValidateAndPrepareAI(IClassicBoard board) {
            _board = board.GetGridCopy();
            _boardValidator.ChangeGrid(_board);

            if (GetOutcomeForPlayer(1).HasValue)
                throw new ArgumentException("Game is already finished. No moves are possible.", nameof(board));
        }

        protected virtual int? GetOutcomeForPlayer(int player) {
            var winner = _boardValidator.GetWinner();
            if (winner == player) {
                return 1;
            } else if (winner == AnotherPlayer(player)) {
                return -1;
            } else if (_boardValidator.IsFull()) {
                return 0;
            } else {
                return null;
            }
        }

        protected virtual int AnotherPlayer(int currentPlayer) {
            return currentPlayer % 2 + 1;
        }

        //TODO using heuristics to firstly choose moves, which have the most potential winning positions, may speed up
        protected virtual List<int> GetEmptyCells() {
            var moves = new List<int>(9);

            for (int index = 0; index < 9; index++) {
                if (_board[index] == 0) {
                    moves.Add(index);
                }
            }

            return moves;
        }
        #endregion
    }
}
