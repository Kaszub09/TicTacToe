using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game
{
    internal class ClassicBoardValidator:BoardValidator {
        internal ClassicBoardValidator() {
        }

        internal ClassicBoardValidator( int[] gridReference) {
            ChangeGrid(gridReference);
        }

        public void ChangeGrid(int[] gridReference) {
            _grid = gridReference;
            _possibleWins = new List<int[]>();
            CalculatePossibleWinCombinations();
        }

        private void CalculatePossibleWinCombinations() {
            //rows
            for (int row = 0; row < 3; row++) {
                var rowsWins = new List<int>();
                for (int col = 0; col < 3; col++) {
                    rowsWins.Add(ClassicCell.GetIndex(row, col));
                }
                _possibleWins.Add(rowsWins.ToArray());
            }
            //columns
            for (int col = 0; col < 3; col++) {
                var colsWins = new List<int>();
                for (int row = 0; row < 3; row++) {
                    colsWins.Add(ClassicCell.GetIndex(row, col));
                }
                _possibleWins.Add(colsWins.ToArray());
            }
            //diagonals
            var diagWins = new List<int>();
            var diag2Wins = new List<int>();
            for (int i = 0; i < 3; i++) {
                diagWins.Add(ClassicCell.GetIndex(i, i));
                diag2Wins.Add(ClassicCell.GetIndex(i, 2-i));
            }
            _possibleWins.Add(diagWins.ToArray());
            _possibleWins.Add(diag2Wins.ToArray());
        }

    }
}
