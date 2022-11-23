using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game {
    internal class MultiBoard :Board, IMultiBoard {
        public Space Space { get; private set; }

        public MultiBoard(Space space) {
            ResetBoard(space);
        }

        public int this[MultiCell cell] {
            get {
                return _grid[cell.Index];
            }
            set {
                _grid[cell.Index] = value;
            }

        }

        public int GetValue(params int[] position) {
            return _grid[Space.GetIndex(position)];
        }
        public void SetValue(int value,params int[] position) {
            _grid[Space.GetIndex(position)] = value;
        }

        public void ResetBoard(Space space) {
            Space = space;
            _grid = new int[Space.SpaceSize];
            _grid.Fill(0);
        }

    }
}


