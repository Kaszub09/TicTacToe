using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game {
    internal abstract class BoardValidator {
        internal ReadOnlyCollection<int[]> PossibleWins => _possibleWins.AsReadOnly();

        protected int[] _grid;
        protected List<int[]> _possibleWins;


        public int GetWinner() {
            foreach (var winsGroup in _possibleWins) {
                //all cells must have the same value
                var value = _grid[winsGroup[0]];
                foreach (var index in winsGroup) {
                    if (_grid[index] != value) {
                        value = -1;
                        break;
                    }
                }
                //if all cells in group are marked by the same player (which is denoted by positive number), he is a winner
                if (value > 0) {
                    return value;
                }
            }
            return 0;
        }

        public int[] GetWinningGroup() {
            foreach (var winsGroup in _possibleWins) {
                //all cells must have the same value
                var value = _grid[winsGroup[0]];
                foreach (var index in winsGroup) {
                    if (_grid[index] != value) {
                        value = -1;
                        break;
                    }
                }
                //if all cells in group are marked by the same player, he is a winner
                if (value > 0) {
                    return winsGroup.ToArray();
                }
            }
            return null;
        }


        public bool IsFull() {
            foreach (var value in _grid) {
                if (value == 0) {
                    return false;
                }
            }
            return true;
        }
    }
}
