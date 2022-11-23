using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.AI
{
    /// <summary>
    /// Classic move with outcome for the player
    /// </summary>
    public struct ClassicMoveEval 
    {
        public ClassicMove Move;
        /// <summary>
        /// Assuming optimal moves form all player, 1 means vicotry, 0 - draw, and -1 loss
        /// </summary>
        public int PlayerOutcome;

        public ClassicMoveEval(int index,int player, int playerOutcome)
        {
            Move = new ClassicMove(index, player);
            PlayerOutcome = playerOutcome;
        }
    }
}
