using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Game
{
    /// <summary>
    /// Classic TicTacToe game with 3x3 board.
    /// </summary>
    public interface IClassicGame {
        /// <summary>
        /// Board board for current game.
        /// </summary>
        IClassicBoard Board { get; }
        /// <summary>
        /// All moves executed so far in current game.
        /// </summary>
        IReadOnlyList<ClassicMove> MovesHistory { get; }
        /// <summary>
        /// Player whose turn it is. 
        /// </summary>
        /// <returns>Either 1 or 2.</returns>
        int NextPlayer { get; }
        bool IsGameFinished { get; }
        /// <summary>
        /// Returns winner of the game or 0 if game ended in draw.
        /// </summary>
        /// <returns>0 if game ended in draw, 1 or 2 otherwise.</returns>
        int Winner { get; }
        /// <summary>
        /// Move is legal if cell is empty and game is not finished. 
        /// </summary>
        /// <param name="index"></param>
        bool IsMoveLegal(int index);
        /// <summary>
        /// Move is legal if cell is empty and game is not finished. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        bool IsMoveLegal(int row, int col);
        /// <summary>
        /// Move is executed only if it's legal. Afterwards state of game is reevalueted.
        /// </summary>
        /// <param name="index"></param>
        bool MakeMove(int index);
        /// <summary>
        /// Move is executed only if it's legal. Afterwards state of game is reevalueted.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        bool MakeMove(int row, int col);
        /// <summary>
        /// Restarts game.
        /// </summary>
        /// <param name="firstPlayer">Must be either 1 or 2</param>
        void StartNewGame(int firstPlayer);
        /// <summary>
        /// Retracts last move, and updates state/winner of the game.
        /// </summary>
        bool RetractLastMove();
        /// <summary>
        /// Gets winning combination if games is finished and wasn't draw.
        /// </summary>
        /// <param name="combination">Null if game is not finished or ended in draw.</param>
        bool GetWinningCombination(out List<ClassicMove> combination);
    }
}