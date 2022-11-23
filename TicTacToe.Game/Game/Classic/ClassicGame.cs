using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game
{
    internal class ClassicGame : IClassicGame {
        public IClassicBoard Board { get { return _board; } }
        public IReadOnlyList<ClassicMove> MovesHistory => _movesHistory.AsReadOnly();

        public int NextPlayer { get; private set; }
        public bool IsGameFinished { get; private set; }
        public int Winner { get; private set; }

        private List<ClassicMove> _movesHistory;
        private ClassicBoard _board;
        private ClassicBoardValidator _boardValidator;

        public ClassicGame() {
            _board = new ClassicBoard();
            _boardValidator = new ClassicBoardValidator(_board.GetGridReference());
            StartNewGame(1);
        }

        public void StartNewGame(int firstPlayer) {
            if (firstPlayer != 1 && firstPlayer != 2) 
                throw new ArgumentException("Value must be 1 or 2", nameof(firstPlayer));

            _board.ResetBoard();
            NextPlayer = firstPlayer;
            IsGameFinished = false;
            Winner = 0;
            _movesHistory = new List<ClassicMove>(9);
        }

        public bool IsMoveLegal(int row, int col) {
            return IsMoveLegal(ClassicCell.GetIndex(row, col));
        }
        public bool IsMoveLegal(int index) {
            return  _board[index] == 0 && (!IsGameFinished);
        }
        public bool MakeMove(int row, int col) {
            return MakeMove(ClassicCell.GetIndex(row, col));
        }
        public bool MakeMove(int index) {
            var isMoveLegal = IsMoveLegal(index);
            if (IsMoveLegal(index)) {
                _board[index] = NextPlayer;
                _movesHistory.Add(new ClassicMove(new ClassicCell(index), NextPlayer));

                Winner = _boardValidator.GetWinner();
                IsGameFinished = Winner > 0 || _board.IsFull();

                ChangePlayer();
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
            ChangePlayer();
    
            return true;
        }

        public bool GetWinningCombination(out List<ClassicMove> combination) {
            if (IsGameFinished && Winner != 0) {
                combination = _boardValidator.GetWinningGroup().Select(x => new ClassicMove(x, Winner)).ToList();
            } else {
                combination = null;
            }
            return combination != null;
        }

        private void ChangePlayer() {
            NextPlayer = NextPlayer % 2 + 1;
        }

    }
}
