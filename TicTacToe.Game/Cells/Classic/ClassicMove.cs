using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Game
{
    /// <summary>
    /// Move denoting position on board and player, who made it.
    /// </summary>
    public struct ClassicMove
    {
        /// <summary>
        /// Position on board.
        /// </summary>
        public ClassicCell Cell { get; private set; }
        /// <summary>
        /// Player who made move.
        /// </summary>
        public int Player { get; private set; }

        internal ClassicMove(int index, int player)
        {
            Cell = new ClassicCell(index);
            Player = player;
        }

        internal ClassicMove(ClassicCell cell, int player)
        {
            Cell = cell;
            Player = player;
        }
    }
}
