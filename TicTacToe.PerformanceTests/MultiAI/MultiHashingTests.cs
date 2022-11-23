using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;

namespace TicTacToe.PerformanceTests {
    internal static class MultiHashingTests {


        public static void General() {
            Console.WriteLine("==========    HashingTests - General Info (4 in 4x4)  ==========");

            var game = Factory.CreateNewGame(2,3,2,2,2);

            var ai = new MultiAI_Hashing();
            MultiMoveEval move;
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Restart();
            move = ai.GetBestMoveRecVal(game);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};" +
                $"AllHits={ai.AllHits}; HashHits={ai.HashHits}; HashRate={((float)ai.HashHits) / ai.AllHits}; DictSize={ai.HashCount};");
            stopwatch.Restart();
            move = ai.GetBestMoveNoRecVal(game);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveNoRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};" +
                $"AllHits={ai.AllHits}; HashHits={ai.HashHits}; HashRate={((float)ai.HashHits) / ai.AllHits}; DictSize={ai.HashCount};");
            Console.WriteLine();
        }

    }
}
