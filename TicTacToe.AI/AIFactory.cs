using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.AI {
    /// <summary>
    /// Every AI can be created from here.
    /// </summary>
    public static class AIFactory {
        /// <summary>
        /// Create AI for evaluating moves classic 3x3 game.
        /// </summary>
        /// <returns></returns>
        public static ClassicAI CreateAI() {
            //For simple 3x3 game AlphaBetaPrunning is the best one;
            //it's better than Hashing in terms of memory and speed for calculating just best move.
            //Also, for just best move difference between recursion and loop appears to be insignificant,
            //but recursion is easier to understand
            return new ClassicAI_AlphaBetaPrunning();
        }
        /// <summary>
        /// Create AI for evaluating moves in custom game with any number od dimensions.
        /// </summary>
        /// <returns></returns>
        public static MultiAI CreateMultiAI() {
            return new MultiAI_AlphaBetaPrunning();
        }
    }
}
