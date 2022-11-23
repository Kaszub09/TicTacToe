using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;
using Xunit;
using Xunit.Abstractions;

namespace TicTacToe.Tests.AITests {
    public class MultiCorrectnessTests {
        private readonly ITestOutputHelper output;

        public MultiCorrectnessTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public void BestMoves() {
            var cGame = Factory.CreateNewGame();
            var mGame = Factory.CreateNewGame(2, 3, 3, 3);

            var cAI = new ClassicAI_AlphaBetaPrunning();
            var mAI = new MultiAI_AlphaBetaPrunning();

            Assert.Equal(cAI.GetBestMove(cGame).Move.Cell.Index, mAI.GetBestMove(mGame).Move.Cell.Index);
            Assert.Equal(cAI.GetBestMove(cGame).PlayerOutcome, mAI.GetBestMove(mGame).PlayerOutcome);

            //var mGame2 = Factory.CreateNewGame(2, 3,2,4,2);
            //mGame2.MakeMoveByIndex(0);
            //var res = mAI.GetBestMoveRecVal(mGame2);
            //res = mAI.GetBestMoveNoRecVal(mGame2);
            //mGame2.MakeMoveByIndex(5);
            //res = mAI.GetBestMoveRecVal(mGame2);
            //res = mAI.GetBestMoveNoRecVal(mGame2);
            //mGame2.MakeMoveByIndex(1);
            //res = mAI.GetBestMoveRecVal(mGame2);
            //res = mAI.GetBestMoveNoRecVal(mGame2);
            //Assert.Equal(1, mAI.GetBestMoveNoRecVal(mGame2).Move.Cell.Index);
            //Assert.Equal(-1, mAI.GetBestMoveNoRecVal(mGame2).PlayerOutcome);

            //var mGame2 = Factory.CreateNewGame(2, 3, 2, 4, 2);
            //mGame2.MakeMoveByIndex(0);
            //output.WriteLine(DateTime.Now.ToString() + "   "+ mAI.GetBestMoveRecVal(mGame2).Move.Cell.Index);
            //output.WriteLine(DateTime.Now.ToString() + "   " + mAI.GetBestMoveNoRecVal(mGame2).Move.Cell.Index);
            //mGame2.MakeMoveByIndex(5);
            //output.WriteLine(DateTime.Now.ToString() + "   " + mAI.GetBestMoveRecVal(mGame2).Move.Cell.Index);
            //output.WriteLine(DateTime.Now.ToString() + "   " + mAI.GetBestMoveNoRecVal(mGame2).Move.Cell.Index);
            //mGame2.MakeMoveByIndex(1);
            //output.WriteLine(DateTime.Now.ToString() + "   " + mAI.GetBestMoveRecVal(mGame2).Move.Cell.Index);
            //output.WriteLine(DateTime.Now.ToString() + "   " + mAI.GetBestMoveNoRecVal(mGame2).Move.Cell.Index);


            cGame.MakeMove(0);
            mGame.MakeMoveByIndex(0);

            cGame.MakeMove(1);
            mGame.MakeMoveByIndex(1);

            cGame.MakeMove(2);
            mGame.MakeMoveByIndex(2);

            cGame.MakeMove(3);
            mGame.MakeMoveByIndex(3);

            Assert.Equal(cAI.GetAllBestMoves(cGame).Select(x=>x.Move.Cell.Index), mAI.GetAllBestMoves(mGame).Select(x => x.Move.Cell.Index));
            Assert.Equal(cAI.GetAllMoves(cGame).Select(x => x.PlayerOutcome), mAI.GetAllMoves(mGame).Select(x => x.PlayerOutcome));

        }
    }
}
