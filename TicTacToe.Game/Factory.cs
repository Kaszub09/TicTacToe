using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Game {
    /// <summary>
    /// Starting point of library, everything needed to play a game can be got from here
    /// </summary>
    public static partial class Factory {
        /// <summary>
        /// Create new classic 3x3 game.
        /// </summary>
        /// <returns></returns>
        public static IClassicGame CreateNewGame() {
            return new ClassicGame();
        }
        /// <summary>
        /// Creates new multidimensional game.
        /// </summary>
        /// <param name="numberOfPlayers">Number of players.</param>
        /// <param name="numOfCellsInLineRequiredToWin">Number of cells player must mark in one line in order to win. Must be greater than 1.</param>
        /// <param name="space">Upper bound for dimensions in space, in which game takes place. {3,3} for classic 3x3 TicTacToe.</param>
        /// <returns></returns>
        public static IMultiGame CreateNewGame(int numberOfPlayers, int numOfCellsInLineRequiredToWin,params int[] space) {
            return new MultiGame(new Space(space), numberOfPlayers, numOfCellsInLineRequiredToWin);
        }
        /// <summary>
        /// Creates new board printer for both classic 3x3 and multidimensional TicTacToe.
        /// </summary>
        /// <returns></returns>
        public static BoardPrinter CreateBoardPrinter() {
            return new BoardPrinter();
        }
    }
}
