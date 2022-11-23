using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TicTacToe.Game {
    /// <summary>
    /// Class used to print given board as string
    /// </summary>
    public class BoardPrinter {

        /// <summary>
        /// Used to translate int values into players characters when converting to string. 
        /// 0 denotes empty cell, positive values players.
        /// Default values are ' ' for empty cell, 'X' and 'O' for first and second player, numbers for other playesr ('3' for third one etc.)
        /// </summary>
        public Dictionary<int, string> PlayerToString { get; private set; } = new Dictionary<int, string>();

        private StringBuilder _sb;

        private Space _space;
        private IMultiBoard _board;
        private List<string> _all2Dboards;

        internal BoardPrinter() {
            PlayerToString[0] = " ";
            PlayerToString[1] = "X";
            PlayerToString[2] = "O";
            _sb = new StringBuilder();
        }

        /// <summary>
        /// Returns given board as string
        /// </summary>
        /// <param name="board">Classic 3x3 board</param>
        public string PrintBoardAsString(IClassicBoard board) {
            _sb.Clear();
            var _maxCellWidth = PlayerToString.Max(pair => pair.Value.Length);

            var firstLine = $" {new String(' ', _maxCellWidth)} | {new String(' ', _maxCellWidth)} | {new String(' ', _maxCellWidth)} ";
            var lastLine = $"_{new String('_', _maxCellWidth)}_|_{new String('_', _maxCellWidth)}_|_{new String('_', _maxCellWidth)}_";

            for (int i = 0; i < 9; i++) {
                if (i % 3 == 0 || i == 8) {
                    _sb.AppendLine(firstLine);
                } else if (i % 3 == 1) {
                    _sb.AppendLine($" {GetChar(board[i - 1], _maxCellWidth)} | {GetChar(board[i], _maxCellWidth)} | {GetChar(board[i + 1], _maxCellWidth)} ");
                } else {
                    _sb.AppendLine(lastLine);
                }
            }

            return _sb.ToString();
        }

        /// <summary>
        /// Returns given board as string. 
        /// For each position in dimensions above second board consisting of first 2 diemnsions is printed. 
        /// Order is descending from highest dimension.
        /// </summary>
        /// <param name="board">Multidimensional board</param>
        public string PrintBoardAsString(IMultiBoard multiBoard) {
            _board = multiBoard;
            _space = multiBoard.Space;
            _all2Dboards = new List<string>();

            PrintSingleBoardDimensionIndex(new int[_space.Dimensions], 2);

            return string.Join("\n", _all2Dboards);
        }

        private void PrintSingleBoardDimensionIndex(int[] position, int dimIdx) {
            if (_space.Dimensions== 1) {
                //Print single 2D board with one column only
                _sb.Clear();
                _sb.AppendLine("-------------------------------------------");
                _sb.AppendLine("Position = [ Row ]");
                _sb.AppendLine("-------------------------------------------");
                _sb.AppendLine(" ___");
                for (int row = 0; row < _space.UBounds[0]; row++) {
                    position[0] = row;
                    for (int i = 0; i < 3; i++) {
                        _sb.Append("|");
                        if (i == 0 ) {
                            _sb.Append("   ");
                        } else if (i == 1) {
                            _sb.Append($" {GetChar(position)} ");
                        } else {
                            _sb.Append("___");
                        }
                        _sb.Append("|");
                        _sb.Append('\n');
                    }
                }

                _all2Dboards.Add(_sb.ToString());

            } else  if (dimIdx == _space.Dimensions) {
                //Print single 2D board
                _sb.Clear();
                _sb.AppendLine("-------------------------------------------");
                _sb.AppendLine("Position = [ Row, Colum" + GetPosAboveSecondDim(position) + "]");
                _sb.AppendLine("-------------------------------------------");

                for (int col = 0; col < _space.UBounds[1]; col++) {
                    _sb.Append(" ___");
                }
                _sb.Append("\n");

                for (int row = 0; row < _space.UBounds[0]; row++) {
                    position[0] = row;

                    for (int i = 0; i < 3; i++) {
                        _sb.Append("|");
                        for (int col = 0; col < _space.UBounds[1]; col++) {
                            position[1] = col;

                            if (i == 0 ) {
                                _sb.Append("   ");
                            } else if (i == 1) {
                                _sb.Append($" {GetChar(position)} ");
                            } else  {
                                _sb.Append("___");
                            }
                            _sb.Append("|");
                        }
                        _sb.Append("\n");
                    }
                }
                _all2Dboards.Add(_sb.ToString());

            } else {
                for (int i = _space.UBounds[dimIdx]-1; i >= 0; i--) {
                    position[dimIdx] = i;
                    PrintSingleBoardDimensionIndex(position, dimIdx + 1);
                }
            }
        }

        private string GetPosAboveSecondDim(int[] position) {
            if (position.Length > 2) {
                return ", " + string.Join(", " ,position.Skip(2).Select(x => x.ToString()));
            } else {
                return " ";
            }
        }

        private string GetChar(int[] position) {
            return GetChar(_board.GetValue(position));
        }

        private string GetChar(int value,int? totalWidth =null) {
            if (!PlayerToString.ContainsKey(value)) 
                PlayerToString[value] = value.ToString();
            
            return totalWidth == null? PlayerToString[value]:PlayerToString[value].PadLeft(totalWidth.Value);
        }
    }
}
