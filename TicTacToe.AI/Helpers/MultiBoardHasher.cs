using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.AI {
    internal class MultiBoardHasher {
        private int[] _grid;
        private StringBuilder _sb = new StringBuilder();
        private List<int[]> _equivalentCombinations;
        private Space _space;

        public MultiBoardHasher() {
        }

        public MultiBoardHasher(Space space,int[] gridReference) {
            ChangeGrid(space,gridReference);
        }
        public void ChangeGrid(Space space, int[] gridReference) {
            _space = space;
            _grid = gridReference;
            CalculateEquivalentHashes();
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
            var hashes = new List<string>(_equivalentCombinations.Count);
            foreach (var combination in _equivalentCombinations) {
                _sb.Clear();
                for (int i = 0; i < combination.Length; i++) {
                    _sb.Append(_grid[combination[i]]);
                }
                _sb.Append(player);
                hashes.Add(_sb.ToString());
            }
            return hashes;
        }

        private void CalculateEquivalentHashes()
        {
            //mirrored with each dim combination sahould cover most of equivalent hashes
            _equivalentCombinations = new List<int[]>((int)Math.Pow(2,_space.Dimensions));
            AddMirrored(Enumerable.Range(0, _grid.Length), new bool[_space.Dimensions], 0);
        }

        private void AddMirrored(IEnumerable<int> allCells, bool[] dimensions, int depth) {
            if (depth >= _space.Dimensions)
            {
                _equivalentCombinations.Add(allCells.Select(x => MirrorCell(x, dimensions)).ToArray());
            }
            else
            {
                dimensions[depth] = true;
                AddMirrored(allCells, dimensions, depth + 1);
                dimensions[depth] = false;
                AddMirrored(allCells, dimensions, depth + 1);
            }
        }

        private int MirrorCell(int index, bool[] dimensions)
        {
            var position = _space.GetPosition(index);
            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] == true)
                {
                    position[i] = _space.UBounds[i] - 1 - position[i];
                }
            }
            return _space.GetIndex(position);
        }
    }
}
