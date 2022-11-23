namespace TicTacToe.Game {
    /// <summary>
    /// Game board for classic 3x3 TicTacToe. 
    /// </summary>
    public interface IClassicBoard {
        /// <summary>
        /// Get value of given cell. Both rows and columns are indexed from 0.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>0 if empty, positive number if marked by a player.</returns>
        int this[int row, int col] { get;  }
        /// <summary>
        /// Get value of given cell. Indexed from 0, left to right, then top to bottom
        /// </summary>
        /// <param name="index"></param>
        /// <returns>0 if empty, positive number if marked by a player.</returns>
        int this[int index] { get; }
        /// <summary>
        /// Gets copy of a grid.
        /// </summary>
        /// <returns></returns>
        int[] GetGridCopy();
        /// <summary>
        /// Checks if all cells are marked.
        /// </summary>
        /// <returns></returns>
        bool IsFull();
    }
}