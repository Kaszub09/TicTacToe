using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Game {
    internal class MultiBoardValidator:BoardValidator {
        private Space _space;
        private int _reqNumOfCells;

        internal MultiBoardValidator() {
        }

        internal MultiBoardValidator(int[] gridReference,Space space,int reqNumOfCells) {
            ChangeGrid(gridReference,space, reqNumOfCells);
        }

        public void ChangeGrid(int[] gridReference, Space space, int reqNumOfCells) {
            _space = space;
            _reqNumOfCells = reqNumOfCells;
            _grid = gridReference;
            _possibleWins = new List<int[]>();
            CalculatePossibleWinCombinations(new int[_reqNumOfCells],0);
        }
        /* Not the fastest method, bu at least it's easy to implement and wrap yor head around. :P 
         * And for small spaces is probably fast enough.
         * 
         * ALTERNATIVE idea is to generate all win combinations for a subspace
         * then calcualte wins for 1 dimension higher (e.g. n) with following rules:
         * - for each cell in each win 
         *      increment n-th dimension position by 1, it's a new win combination do this till you hit upper bound for n-th dim
         *   //this gives you all columns if you started with first dim of 3x3
         *   
         * - for each combination  
         *      increment first cell's first dim pos by 0, second cell's second dim pos by 1 etc.   
         *   then start again with 1 for first cell, 2 for second etc. Repeat till you hit upper bound of any dimension.
         *   then repeat, bt in reverse order
         *  //this gives you all diagonals if you started with first dim of 3x3
         *  
         * - for each position in subspace
         *      increment n-th dimension by 1 starting from 0 to number of cells to win - it's new group
         *   then repaet strating form 1, 2 etc.

         */
        private void CalculatePossibleWinCombinations(int[] cellCombination, int depth) {
            if(depth== _reqNumOfCells) {
                if(AreCellsInLine(cellCombination.Select(x=>new MultiCell(_space, x)).ToArray())) {
                    if (!IsAlreadyInList( cellCombination.Reverse().ToArray()))
                        _possibleWins.Add(cellCombination.ToArray());
                }

            } else {
                for (int i = 0;i< _space.SpaceSize;i++) {
                    var duplicateExists = false;
                    for(int k = 0; k < depth; k++) {
                        if (cellCombination[k] == i) {
                            duplicateExists = true;
                            break;
                        }
                    }

                    if (!duplicateExists) {
                        cellCombination[depth] = i;
                        CalculatePossibleWinCombinations(cellCombination, depth + 1);
                    }
                }
            }
        }

        private  bool IsAlreadyInList(int[] array) {
            foreach(var win in _possibleWins) {
                var isTheSame = true;
                for (int i = 0; i < win.Length; i++) {
                    if (win[i] != array[i]) {
                        isTheSame = false;
                        break;
                    }
                }
                if (isTheSame)
                    return true;
            }
            
            return false;

        }

        //In line in order from first to last cell
        private bool AreCellsInLine(MultiCell[] cellCombination) {
            //basically each dimension must be in line
            for(int dimIdx = 0; dimIdx < _space.Dimensions; dimIdx++) {
                //We assume  that winning combination must have ate least 2 cells, otherwise game doesn''t make sense
                //since it's always finish after first move
                var dimDiff = cellCombination[1][dimIdx] - cellCombination[0][dimIdx];
                //for cells to be in line, difference between cells in one dimension must be the same and equal -1,0 or 1
                if (dimDiff < -1 || dimDiff > 1)
                    return false;

                for (int i = 2; i < cellCombination.Length; i++) {
                    if (cellCombination[i][dimIdx] - cellCombination[i-1][dimIdx] != dimDiff)
                        return false;
                }
            }
            return true;
        }


    }
}