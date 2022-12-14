using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;

namespace TicTacToe.PerformanceTests {
    internal static class SimplePruningTests {


        public static void General() {
            Console.WriteLine("==========   SimplePruningTests - General Info   ==========");

            var game = Factory.CreateNewGame();

            var ai = new ClassicAI_SimplePrunning();
            ClassicMoveEval move;
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Restart();
            move = ai.GetBestMoveRecVal(game.Board, 1);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};");

            stopwatch.Restart();
            move = ai.GetBestMoveRecStruct(game.Board, 1);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveRecStruct; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};");

            stopwatch.Restart();
            move = ai.GetBestMoveNoRecVal(game.Board, 1);
            stopwatch.Stop();
            Console.WriteLine($"GetBestMoveNoRecVal; Move={move.Move.Cell.Index}; Outcome={move.PlayerOutcome}; Tick={stopwatch.Elapsed.Ticks}; Ms={stopwatch.Elapsed.TotalMilliseconds};");

            Console.WriteLine();
        }

        public static void Time(int repetitions = 100) {
            Console.WriteLine("==========   SimplePruningTests - Time  ==========");

            var bp = Factory.CreateBoardPrinter();
            var game = Factory.CreateNewGame();
            var ai = new ClassicAI_SimplePrunning();
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
                move = ai.GetBestMoveRecStruct(game.Board, 1);
                stopwatch.Stop();
                timeCounter[1].Add(stopwatch.Elapsed);

                stopwatch.Restart();
                move = ai.GetBestMoveNoRecVal(game.Board, 1);
                stopwatch.Stop();
                timeCounter[2].Add(stopwatch.Elapsed);
            }

            Console.WriteLine($"GetBestMoveRecVal; Ticks={timeCounter[0].Average(x => x.Ticks)}; Ms={timeCounter[0].Average(x => x.TotalMilliseconds)}");
            Console.WriteLine($"GetBestMoveRecStruct; Ticks={timeCounter[1].Average(x => x.Ticks)}; Ms={timeCounter[1].Average(x => x.TotalMilliseconds)}");
            Console.WriteLine($"GetBestMoveNoRecVal; Ticks={timeCounter[2].Average(x => x.Ticks)}; Ms={timeCounter[2].Average(x => x.TotalMilliseconds)}");
            Console.WriteLine();
        }


    }
}
