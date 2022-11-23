using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests.AITests {
    public class LoopCorrectnessTests {

        public static IList<int[]> CreateRandomGames(int count=100,int maxVal = 9) {
            Random rand = new Random();
            var games = new List<int[]>();
            for (int i = 0; i < count; i++) {
                games.Add(new int[] { rand.Next(maxVal), rand.Next(maxVal), rand.Next(maxVal), rand.Next(maxVal),
                    rand.Next(maxVal), rand.Next(maxVal), rand.Next(maxVal), rand.Next(maxVal), rand.Next(maxVal) });
            }
            return games;
        }

        [Fact]
        public void CheckRandomGames() {
            var games = CreateRandomGames(100);

            var game = Factory.CreateNewGame();
            var aiSimple = new ClassicAI_SimplePrunning();
            var aiAlphaBeta = new ClassicAI_AlphaBetaPrunning();
            var aiHash = new ClassicAI_Hashing();

            for (int i = 0; i < games.Count; i++) {
                var moves = games[i];
                game.StartNewGame(1);
                var idx = 0;

                while (!game.IsGameFinished && idx < moves.Length) {
                    var bestMove = aiAlphaBeta.GetBestMoveRecVal(game.Board, game.NextPlayer);

                    Assert.Equal(bestMove, aiSimple.GetBestMoveNoRecVal(game.Board, game.NextPlayer));
                    Assert.Equal(bestMove, aiAlphaBeta.GetBestMoveNoRecVal(game.Board, game.NextPlayer));
                    Assert.Equal(bestMove, aiHash.GetBestMoveNoRecVal(game.Board, game.NextPlayer));

                    game.MakeMove(moves[idx]);
                    idx++;
                }
            }
        }

        [Fact]
        public void CheckRandomGamesMulti() {
            var games = CreateRandomGames(1);

            var game = Factory.CreateNewGame(2,4,4,4);
            var aiAlphaBeta = new MultiAI_AlphaBetaPrunning();

            for (int i = 0; i < games.Count; i++) {
                var moves = games[i];
                game.StartNewGame(1);
                var idx = 0;

                while (!game.IsGameFinished && idx < moves.Length) {
                    var bestMove = aiAlphaBeta.GetBestMoveRecVal(game);
                    var bestMoveRec = aiAlphaBeta.GetBestMoveNoRecVal(game);
                    Assert.Equal(bestMove.Move.Cell.Index, bestMoveRec.Move.Cell.Index);
                    Assert.Equal(bestMove.PlayerOutcome, bestMoveRec.PlayerOutcome);
                    game.MakeMoveByIndex(moves[idx]);
                    idx++;
                }
            }
        }


        //[Fact]
        public void CheckAllGames() {
            var game = Factory.CreateNewGame();
            var aiSimple = new ClassicAI_SimplePrunning();
            var aiAlphaBeta = new ClassicAI_AlphaBetaPrunning();
            var aiHash = new ClassicAI_Hashing();

            int depth = 0;
            int[] history = new int[10];
            history[0] = 0;
            int counter = 0;

            while (depth >= 0) {
                if (game.IsGameFinished || history[depth] > 8 || depth>9) {
                    if (counter % 1000 == 0) {
                        Debug.Print(string.Join("_", history.Select(x => x.ToString()))); 
                    }
                    counter++;

                    if (depth > 0) {
                        depth--;
                        game.RetractLastMove();
                        history[depth]++;
                    } else {
                        depth--;
                    }

                } else if (game.Board[history[depth]] == 0) {
                    var bestMove = aiAlphaBeta.GetBestMoveRecVal(game.Board, game.NextPlayer);

                    Assert.Equal(bestMove, aiSimple.GetBestMoveNoRecVal(game.Board, game.NextPlayer));
                    Assert.Equal(bestMove, aiAlphaBeta.GetBestMoveNoRecVal(game.Board, game.NextPlayer));
                    Assert.Equal(bestMove, aiHash.GetBestMoveNoRecVal(game.Board, game.NextPlayer));

                    game.MakeMove(history[depth]);
                    depth++;
                    history[depth] = 0;

                } else {
                    history[depth]++;
                }
            }
        }
    }
}
