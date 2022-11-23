using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game {
    internal abstract class Board {
        protected int[] _grid = new int[9];

        public int this[int index] {
            get {
                return _grid[index];
            }
            set {
                _grid[index] = value;
            }

        }

        public Board() {
            ResetBoard();
        }

        public void ResetBoard() {
            _grid.Fill(0);
        }

        public int[] GetGridReference() {
            return _grid;
        }
        public int[] GetGridCopy() {
            return _grid.ToArray();
        }

        public bool IsEmpty(int index) {
            return _grid[index] == 0;
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
