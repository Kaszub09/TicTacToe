using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;

namespace TicTacToe.PerformanceTests {
    internal static class MultiAlphaBetaPrunningTests {


        public static void General() {
            Console.WriteLine("==========    AlphaBetaPrunningTests - General Info (4 in 4x4x4)  ==========");

            var game = Factory.CreateNewGame(2,3,4,4);

            var ai = new MultiAI_AlphaBetaPrunning();
            MultiMoveEval move;
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Restart();
            move = ai.GetBestMoveRecVal(game);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};");

            stopwatch.Restart();
            move = ai.GetBestMoveNoRecVal(game);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveNoRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};");
            Console.WriteLine();
        }

    }
}
