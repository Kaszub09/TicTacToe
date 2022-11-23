using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe.Game
{
    /// <summary>
    /// Cell denoting position in multidimensional space, as well as on board created in this space.
    /// </summary>
    public class MultiCell
    {
        /// <summary>
        /// Space to which the cell belongs.
        /// </summary>
        public Space Space { get; private set; }
        /// <summary>
        /// Position of the cell in space.
        /// </summary>
        public int[] Position { get; private set; }
        /// <summary>
        /// Position of the cell in space represented by single number. 
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// Returns position of the cell in given dimension
        /// </summary>
        public int this[int dimension] {
            get {
                return Position[dimension];
            }
        }

        internal MultiCell(Space space) {
            Space = space;
        }

        internal MultiCell(Space space,params int[] position):this(space) {
            if(!space.IsPositionInSpace(position))
                throw new ArgumentOutOfRangeException(nameof(position),"Position does not belong to space.");

            Position = position.ToArray();
            Index = space.GetIndex(position);
        }

        internal MultiCell(Space space, int index) : this(space) {
            if (!space.IsIndexInSpace(index))
                throw new ArgumentOutOfRangeException(nameof(index), "index does not belong to space.");

            Position = space.GetPosition(index);
            Index = index;
        }

    }
}
