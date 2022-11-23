using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Game
{
    /// <summary>
    /// Move denoting position on board and player, who made it
    /// </summary>
    public class MultiMove
    {
        /// <summary>
        /// Position on board
        /// </summary>
        public MultiCell Cell { get; private set; }
        /// <summary>
        /// Player who made move
        /// </summary>
        public int Player { get; private set; }

        internal MultiMove(Space space,int index, int player)
        {
            Cell = new MultiCell(space,index);
            Player = player;
        }

        internal MultiMove(MultiCell cell, int player)
        {
            Cell = cell;
            Player = player;
        }
    }
}
