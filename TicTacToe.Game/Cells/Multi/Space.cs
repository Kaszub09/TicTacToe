using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe.Game
{
    /// <summary>
    /// Space in which the game takes place.
    /// </summary>
    public class Space
    {
        /// <summary>
        /// Number of elements in space.
        /// </summary>
        public int SpaceSize { get; private set; }
        /// <summary>
        /// Number of dimensions in space.
        /// </summary>
        public int Dimensions { get; private set; }
        /// <summary>
        /// Upper bounds (exclusive) for all dimensions.
        /// For each dimension possible values are in range [0,UBounds(dimension))
        /// </summary>
        public int[] UBounds { get; private set; }

        /// <summary>
        /// Used to change int[] position array into single int index and vice versa
        /// </summary>
        private int[] _dimMultiplier;

        internal Space(params int[] upperBounds){
            Dimensions = upperBounds.Length;
            UBounds = upperBounds.ToArray();

            _dimMultiplier = new int[Dimensions];
            _dimMultiplier[Dimensions - 1] = 1;
            for (int i = Dimensions-2; i >=0; i--) {
                _dimMultiplier[i] = UBounds[i+1] *_dimMultiplier[i + 1];
            }

            SpaceSize = _dimMultiplier[0] * UBounds[0];
        }

        public bool IsIndexInSpace(int index) {
            return 0<= index && index < SpaceSize;
        }

        public bool IsCellInSpace(MultiCell multiCell) {
            return IsIndexInSpace(multiCell.Index);
        }
        
        public bool IsPositionInSpace(params int[] position) {
            if (position.Length != Dimensions)
                return false;

            for (int i = 0; i < Dimensions; i++) {
                if (position[i] > UBounds[i] || position[i] < 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets position in given dimension from position denoted by single integer.
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetDimPosition(int dim, int index) {
            return dim == 0? index / _dimMultiplier[dim] : (index % _dimMultiplier[dim - 1]) / _dimMultiplier[dim];
        }

        public int[] GetPosition( int index) {
            var position = new int[Dimensions];
            for (int i = 0; i < Dimensions; i++) {
                position[i] = GetDimPosition(i, index);
            }
            return position;
        }
        /// <summary>
        /// Translates position into one denoted by single integer.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetIndex(int[] position) {
            var index = 0;
            for (int i = 0; i < Dimensions; i++) {
                index += position[i]*_dimMultiplier[i];
            }
            return index;
        }

    }
}
