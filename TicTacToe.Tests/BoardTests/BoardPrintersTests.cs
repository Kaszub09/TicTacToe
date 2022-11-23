using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game;
using Xunit;
using System.Diagnostics;
using Xunit.Abstractions;

namespace TicTacToe.Tests.BoardTests {
    public class BoardPrintersTests {
        private BoardPrinter _mBoardPrinter = new BoardPrinter();

        private readonly ITestOutputHelper output;

        public BoardPrintersTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public void Print2Dboard() {
            var board = new ClassicBoard();
            board[2] = 1;
            board[3] = 2;
            board[6] = 1;
            output.WriteLine("2D  = [0,2]x[0,2]");
            output.WriteLine("=================================================");
            output.WriteLine(_mBoardPrinter.PrintBoardAsString(board));
        }
        [Fact]
        public void PrintMultiboard() {
            var mBoard = new MultiBoard(new Space(4));
            mBoard[2] = 1;
            mBoard[3] = 2;
            output.WriteLine("1D  = [0,3]");
            output.WriteLine("=================================================");
            output.WriteLine(_mBoardPrinter.PrintBoardAsString(mBoard));

             mBoard = new MultiBoard(new Space(2,2));
            mBoard.SetValue(1,0,1);
            output.WriteLine("2D  = [0,1]x[0,1]");
            output.WriteLine("=================================================");
            output.WriteLine(_mBoardPrinter.PrintBoardAsString(mBoard));


            mBoard = new MultiBoard(new Space(3, 3, 2));
            mBoard[2] = 1;
            mBoard.SetValue(2, 1, 1, 0);
            mBoard.SetValue(2, 1, 1, 1);
            output.WriteLine("3D  = [0,2]x[0,2]x[0,1]");
            output.WriteLine("=================================================");
            output.WriteLine(_mBoardPrinter.PrintBoardAsString(mBoard));


            mBoard = new MultiBoard(new Space(2, 3, 2,4));
            mBoard[2] = 1;
            mBoard.SetValue(2, 1, 1, 0, 3);
            mBoard.SetValue(2, 1, 1, 1,2);
            output.WriteLine("4D  = [0,1]x[0,2]x[0,1]x[0,3]");
            output.WriteLine("=================================================");
            output.WriteLine(_mBoardPrinter.PrintBoardAsString(mBoard));

        }

    }
}
