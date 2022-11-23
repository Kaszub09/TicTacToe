

namespace TicTacToe.Game {
    /// <summary>
    /// Game board for multidemnsional TicTacToe.
    /// </summary>
    public interface IMultiBoard {
        /// <summary>
        /// Space of the board.
        /// </summary>
        Space Space { get; }
        /// <summary>
        /// Get value of given cell. 
        /// </summary>
        /// <returns>0 if empty, positive number if marked by a player.</returns>
        int this[int index] { get;  }
        /// <summary>
        /// Get value of given cell.
        /// </summary>
        /// <returns>0 if empty, positive number if marked by a player.</returns>
        int this[MultiCell cell] { get; }
        /// <summary>
        /// Get value of given cell. All dimensions are indexed from 0.  Order is last to first dimension, min to max position in dimension. E.g (0,0,0),(0,0,1),(0,1,0),(0,1,1),(1,0,0)... for 2x2x2 grid
        /// </summary>
        /// <returns>0 if empty, positive number if marked by a player.</returns>
        public int GetValue(params int[] position);
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