using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Game {
    /// <summary>
    /// TicTacToe game with multidimensional board
    /// </summary>
    public interface IMultiGame {
        /// <summary>
        /// Multidimensional board for current game.
        /// </summary>
        IMultiBoard Board { get; }
        /// <summary>
        /// All moves executed so far in current game.
        /// </summary>
        IReadOnlyList<MultiMove> MovesHistory { get; }
        /// <summary>
        /// Number of players in current game.
        /// </summary>
        int NumberOfPlayers { get; }
        /// <summary>
        /// Number of cells required to be in single line in order for player to win.
        /// </summary>
        int NumOfCellsInLineRequiredToWin { get; }

        bool IsGameFinished { get; }
        /// <summary>
        /// Player whose turn it is. Players are numbered from 1 to NumberOfPlayers (inclusive)
        /// </summary>
        int NextPlayer { get; }
        /// <summary>
        /// Returns player number if any player won. Otherwise reutrns 0. If game is finished, 0 denotes draw.
        /// </summary>
        int Winner { get; }

        bool GetWinningCombination(out List<MultiMove> combination);
        /// <summary>
        /// Move is legal if cell is empty and game is not finished. 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool IsMoveLegalByIndex(int index);
        /// <summary>
        /// Move is legal if cell is empty and game is not finished. 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool IsMoveLegalByPosition(params int[] position);
        /// <summary>
        /// Move is executed only if it's legal. Afterwards state of game is reevalueted.
        /// </summary>
        /// <param name="index"></param>
        /// <returns><see langword="true"/> if successful,<see langword="false"/>  otherwise</returns>
        bool MakeMoveByIndex(int index);
        /// <summary>
        /// Move is executed only if it's legal. Afterwards state of game is reevalueted.
        /// </summary>
        /// <param name="position"></param>
        /// <returns><see langword="true"/> if successful,<see langword="false"/>  otherwise</returns>
        bool MakeMoveByPosition(params int[] position);
        /// <summary>
        /// Retracts last move, and updates state/winner of the game.
        /// </summary>
        /// <returns><see langword="true"/> if successful,<see langword="false"/>  otherwise</returns>
        bool RetractLastMove();
        /// <summary>
        /// Restarts game.
        /// </summary>
        /// <param name="firstPlayer">Must be between 1 and NumberOfPlayers (inclusive)</param>
        void StartNewGame(int firstPlayer);
    }
}