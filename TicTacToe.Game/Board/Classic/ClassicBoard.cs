using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game
{
    internal class ClassicBoard :Board, IClassicBoard {
        public int this[int row, int col] {
            get {
                return _grid[ClassicCell.GetIndex(row,col)];
            }
            set {
                _grid[ClassicCell.GetIndex(row, col)] = value;
            }

        }

    }
}
