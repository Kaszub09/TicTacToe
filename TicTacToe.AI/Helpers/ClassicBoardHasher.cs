using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.AI {
    internal class ClassicBoardHasher
    {
        private int[] _grid;
        private StringBuilder _sb = new StringBuilder();
        private List<int[]> _equivalentCombinations = new List<int[]>(){
            //Mirror row
            new int[] { 6, 7, 8, 3, 4, 5, 0, 1, 2 },
            //Mirror col
            new int[] { 2, 1, 0, 5, 4, 3, 8, 7, 6 },
            //Rotations, 90,180,270
            new int[] { 6, 3, 0, 7, 4, 1, 8, 5, 2 },
            new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },
            new int[] { 2, 5, 8, 1, 4, 7, 0, 3, 6 },
            //Diagonals 0<->8, 2<->6
            new int[] { 8, 5, 2, 7, 4, 1, 6, 3, 0 },
            new int[] { 0, 3, 6, 1, 4, 7, 2, 5, 8 }
        };


        public ClassicBoardHasher() {
        }

        public ClassicBoardHasher(int[] gridReference) {
            ChangeGrid(gridReference);
        }
        public void ChangeGrid(int[] gridReference) {
            _grid = gridReference;
        }
        public string GetHash(int player) {
            _sb.Clear();
            for (int i = 0; i < _grid.Length; i++) {
                _sb.Append(_grid[i]);
            }
            _sb.Append(player);
            return _sb.ToString();
        }

        public List<string> GetAllEquivalentHashes(int player) {
            var hashes = new List<string>(8);
            foreach (var combination in _equivalentCombinations) {
                _sb.Clear();
                for (int i = 0; i < combination.Length; i++) {
                    _sb.Append(_grid[combination[i]]);
                }
                _sb.Append(player);
                hashes.Add(_sb.ToString());
            }
            hashes.Add(GetHash(player));
            return hashes;
        }

    }
}
