using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game
{
    internal class MultiGame : IMultiGame {
        public IMultiBoard Board { get { return _board; } }
        public IReadOnlyList<MultiMove> MovesHistory => _movesHistory.AsReadOnly();

        public int NumberOfPlayers { get; private set; }
        public int NumOfCellsInLineRequiredToWin { get; private set; }

        public int NextPlayer { get; private set; }
        public bool IsGameFinished { get; private set; }
        public int Winner { get; private set; }

        private Space _space;
        private MultiBoard _board;
        private MultiBoardValidator _boardValidator;
        private List<MultiMove> _movesHistory;

        public MultiGame(Space space, int numberOfPlayers, int numOfCellsInLineRequiredToWin) {
            _space = space;
            NumberOfPlayers = numberOfPlayers;
            NumOfCellsInLineRequiredToWin = numOfCellsInLineRequiredToWin;

            _board = new MultiBoard(space);
            _boardValidator = new MultiBoardValidator(_board.GetGridReference(), space, numOfCellsInLineRequiredToWin);
            StartNewGame(1);
        }

        public void StartNewGame(int firstPlayer) {
            if (firstPlayer < 1 || firstPlayer > NumberOfPlayers)
                throw new ArgumentException("Value must be between 1 and total number of players (inclusive)", nameof(firstPlayer));

            NextPlayer = firstPlayer;
            IsGameFinished = false;
            Winner = 0;

            _board.ResetBoard();
            _movesHistory = new List<MultiMove>();
        }

        public bool IsMoveLegalByPosition(params int[] position) {
            return _space.IsPositionInSpace(position) && IsMoveLegalByIndex(_space.GetIndex(position));
        }
        public bool IsMoveLegalByIndex(int index) {
            return _board[index] == 0 && (!IsGameFinished) && _space.IsIndexInSpace(index);
        }
        public bool MakeMoveByPosition(params int[] position) {
            return MakeMoveByIndex(_space.GetIndex(position));
        }
        public bool MakeMoveByIndex(int index) {
            var isMoveLegal = IsMoveLegalByIndex(index);
            if (isMoveLegal) {
                _board[index] = NextPlayer;
                _movesHistory.Add(new MultiMove(new MultiCell(_space, index), NextPlayer));

                Winner = _boardValidator.GetWinner();
                IsGameFinished = Winner > 0 || _board.IsFull();

                UpdateNextPlayer();
            }
            return isMoveLegal;
        }

        public bool RetractLastMove() {
            if (_movesHistory.Count == 0)
                return false;

            var lastMove = _movesHistory.Last();

            _movesHistory.Remove(lastMove);
            _board[lastMove.Cell.Index] = 0;

            Winner = _boardValidator.GetWinner();
            IsGameFinished = Winner > 0 || _board.IsFull();
            UpdateNextPlayer();

            return true;
        }

        public bool GetWinningCombination(out List<MultiMove> combination) {
            if (IsGameFinished && Winner != 0) {
                combination = _boardValidator.GetWinningGroup().Select(x => new MultiMove(_space, x, Winner)).ToList();
            } else {
                combination = null;
            }
            return combination != null;
        }

        private void UpdateNextPlayer() {
            NextPlayer = NextPlayer % NumberOfPlayers + 1;
        }

    }
}
