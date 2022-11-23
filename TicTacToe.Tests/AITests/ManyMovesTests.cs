using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests.AITests {
    public class ManyMovesTests {
        [Fact]
        public void AllMoves() {
            var game = Factory.CreateNewGame();
            var aiSimple = new ClassicAI_SimplePrunning();
            var aiAlphaBeta = new ClassicAI_AlphaBetaPrunning();
            var aiHash = new ClassicAI_Hashing();

            var allMoves = new List<ClassicMoveEval>() {
                new ClassicMoveEval(0, 1, 0),
                new ClassicMoveEval(1, 1, 0),
                new ClassicMoveEval(2, 1, 0),
                new ClassicMoveEval(3, 1, 0),
                new ClassicMoveEval(4, 1, 0),
                new ClassicMoveEval(5, 1, 0),
                new ClassicMoveEval(6, 1, 0),
                new ClassicMoveEval(7, 1, 0),
                new ClassicMoveEval(8, 1, 0)
            };

            Assert.Equal(allMoves, aiSimple.GetAllMoves(game));
            Assert.Equal(allMoves, aiAlphaBeta.GetAllMoves(game));
            Assert.Equal(allMoves, aiHash.GetAllMoves(game));

        }

        [Fact]
        public void BestMoves() {
            var game = Factory.CreateNewGame();
            var aiSimple = new ClassicAI_SimplePrunning();
            var aiAlphaBeta = new ClassicAI_AlphaBetaPrunning();
            var aiHash = new ClassicAI_Hashing();

            game.MakeMove(4);
            game.MakeMove(1);
            var allBestMoves = new List<ClassicMoveEval>() {
                new ClassicMoveEval(0, 1, 1),
                new ClassicMoveEval(2, 1, 1),
                new ClassicMoveEval(3, 1, 1),
                new ClassicMoveEval(5, 1, 1),
                new ClassicMoveEval(6, 1, 1),
                new ClassicMoveEval(8, 1, 1)
            };

            Assert.Equal(allBestMoves, aiSimple.GetAllBestMoves(game));
            Assert.Equal(allBestMoves, aiAlphaBeta.GetAllBestMoves(game));
            Assert.Equal(allBestMoves, aiHash.GetAllBestMoves(game));

            game.MakeMove(0);
            var allMoves = new List<ClassicMoveEval>() {
                new ClassicMoveEval(2, 2, -1),
                new ClassicMoveEval(3, 2, -1),
                new ClassicMoveEval(5, 2, -1),
                new ClassicMoveEval(6, 2, -1),
                new ClassicMoveEval(7, 2, -1),
                new ClassicMoveEval(8, 2, -1)
            };
            Assert.Equal(allMoves, aiSimple.GetAllBestMoves(game));
            Assert.Equal(allMoves, aiAlphaBeta.GetAllBestMoves(game));
            Assert.Equal(allMoves, aiHash.GetAllBestMoves(game));
            Assert.Equal(allMoves, aiSimple.GetAllMoves(game));
            Assert.Equal(allMoves, aiAlphaBeta.GetAllMoves(game));
            Assert.Equal(allMoves, aiHash.GetAllMoves(game));

        }
    }
}
