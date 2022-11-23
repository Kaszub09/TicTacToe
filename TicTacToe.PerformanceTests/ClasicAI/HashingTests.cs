using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;

namespace TicTacToe.PerformanceTests {
    internal static class HashingTests {


        public static void General() {
            Console.WriteLine("========== HashingTests - General Info    ==========");

            var game = Factory.CreateNewGame();

            var ai = new ClassicAI_Hashing();
            ClassicMoveEval move;
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Restart();
            move = ai.GetBestMoveRecVal(game.Board, 1);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};" +
                $"AllHits={ai.AllHits}; HashHits={ai.HashHits}; HashRate={((float)ai.HashHits)/ ai.AllHits}; DictSize={ai.HashCount};");

            stopwatch.Restart();
            move = ai.GetBestMoveNoRecVal(game.Board, 1);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveNoRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};" +
                $"AllHits={ai.AllHits}; HashHits={ai.HashHits}; HashRate={((float)ai.HashHits) / ai.AllHits}; DictSize={ai.HashCount};"); ;
            Console.WriteLine();
        }

        public static void Time(int repetitions = 100) {
            Console.WriteLine("==========   HashingTests - Time   ==========");

            var bp = Factory.CreateBoardPrinter();
            var game = Factory.CreateNewGame();
            var ai = new ClassicAI_Hashing();
            ClassicMoveEval move;
            Stopwatch stopwatch = new Stopwatch();

            var timeCounter = new List<List<TimeSpan>>();
            for (int i = 0; i < 3; i++) {
                timeCounter.Add(new List<TimeSpan>());
            };

            for (int i = 0; i < repetitions; i++) {
                stopwatch.Restart();
                move = ai.GetBestMoveRecVal(game.Board, 1);
                stopwatch.Stop();
                timeCounter[0].Add(stopwatch.Elapsed);

                stopwatch.Restart();
                move = ai.GetBestMoveNoRecVal(game.Board, 1);
                stopwatch.Stop();
                timeCounter[2].Add(stopwatch.Elapsed);
            }

            Console.WriteLine($"GetBestMoveRecVal; Ticks={timeCounter[0].Average(x => x.Ticks)}; Ms={timeCounter[0].Average(x => x.TotalMilliseconds)}");
            Console.WriteLine($"GetBestMoveNoRecVal; Ticks={timeCounter[2].Average(x => x.Ticks)}; Ms={timeCounter[2].Average(x => x.TotalMilliseconds)}");
            Console.WriteLine();
        }

  

    }
}
