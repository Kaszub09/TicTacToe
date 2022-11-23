using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.AI {
    /// <summary>
    /// Base of classes for finding best move for a player
    /// </summary>
    public abstract class MultiAI {
        /// <summary>
        /// Maximum depth when searching game tree. 
        /// If value is greater than number of cells on board, all result are accurate.
        /// If not, heuristics are used to try and evaulate board, if  depth is reached and outcome can't be determined.
        /// In this case player with most possibilities to complete line is considered winner.
        /// </summary>
        public int MaxDepth { get; set; } = 9;
        protected const int BestPossibleMoveValue = 1;

        protected int _numberOfPlayers;
        protected int[] _board;

        internal MultiBoardValidator _boardValidator;

        internal MultiAI() {
            _boardValidator = new MultiBoardValidator();
        }

        #region GetMoves
        public abstract MultiMoveEval GetBestMove(IMultiGame game);
        public abstract List<MultiMoveEval> GetAllBestMoves(IMultiGame game);
        public abstract List<MultiMoveEval> GetAllMoves(IMultiGame game);


        internal abstract MultiMoveEval GetBestMoveRecVal(IMultiGame game);
        internal abstract MultiMoveEval GetBestMoveNoRecVal(IMultiGame game);
        #endregion

        #region HelpersFunctions
        protected virtual void ValidateAndPrepareAI(IMultiGame game) {
            if (game.IsGameFinished)
                throw new ArgumentException("Game is already finished. No moves are possible.", nameof(game));

            _board = game.Board.GetGridCopy();
            _boardValidator.ChangeGrid(_board, game.Board.Space,game.NumOfCellsInLineRequiredToWin);
            _numberOfPlayers = game.NumberOfPlayers;
        }

        protected virtual int? GetOutcomeForPlayer(int player) {
            var winner = _boardValidator.GetWinner();
            if (winner == player) {
                return 1;
            } else if (winner!=0 && winner!=player) {
                return -1;
            } else if (_boardValidator.IsFull()) {
                return 0;
            } else {
                return null;
            }
        }

        protected virtual int NextPlayer(int currentPlayer) {
            return currentPlayer == _numberOfPlayers ? 1 : currentPlayer + 1;
        }

        protected virtual int PreviousPlayer(int currentPlayer) {
            return currentPlayer==1? _numberOfPlayers:currentPlayer -1 ;
        }

        //TODO using heuristics to firstly choose moves, which have the most potential winning positions, may speed up
        protected virtual List<int> GetEmptyCells() {
            var moves = new List<int>();

            for (int index = 0; index < _board.Length; index++) {
                if (_board[index] == 0) {
                    moves.Add(index);
                }
            }

            return moves;
        }
        #endregion
    }
}
