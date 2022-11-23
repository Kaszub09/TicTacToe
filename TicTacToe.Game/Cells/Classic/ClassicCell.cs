using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace TicTacToe.Game
{
    /// <summary>
    /// Cell denoting position on classic 3x3 board.
    /// </summary>
    public struct ClassicCell
    {
        /// <summary>
        /// Index of cell on board, indexed from 0 in order left->right, then top->bottom.
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// Row of cell on board, indexed from 0.
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// Column of cell on board, indexed from 0.
        /// </summary>
        public int Col { get; private set; }

        internal ClassicCell(int row, int col)
        {
            Row = row;
            Col = col;
            Index = GetIndex(row, col);
        }

        internal ClassicCell(int index)
        {
            Row = GetRow(index);
            Col = GetCol(index);
            Index = index;
        }
        /// <summary>
        /// Tanslates given row and column to index
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>Index in range [0,8]</returns>
        public static int GetIndex(int row, int col)
        {
            return row * 3 + col;
        }
        /// <summary>
        /// Calculates row from given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Row in range [0,2]</returns>
        public static int GetRow(int index)
        {
            return index / 3;
        }
        /// <summary>
        /// Calcualtes column from given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Column in range [0,2]</returns>
        public static int GetCol(int index)
        {
            return index % 3;
        }
    }
}
