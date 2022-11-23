using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.AI
{
    /// <summary>
    /// Move in multidimensional game with outcome for  the player
    /// </summary>
    public class MultiMoveEval 
    {
        public MultiMove Move;
        /// <summary>
        /// Assuming optimal moves form all player, 1 means vicotry, 0 - draw, and -1 loss
        /// </summary>
        public int PlayerOutcome;

        public MultiMoveEval(Space space,int index,int player, int playerOutcome)
        {
            Move = new MultiMove(space,index, player);
            PlayerOutcome = playerOutcome;
        }
    }
}
