using BenchmarkDotNet.Running;
using System.Diagnostics;
using TicTacToe.AI;
using TicTacToe.Game;

namespace TicTacToe.PerformanceTests {
    internal class Program {
        static void Main(string[] args) {
            //ClassicAI_SimpleTests();
            //MultiAI_SimpleTests();
            BenchamarkDotNet();

        }
        private static void BenchamarkDotNet() {
            //Looks like AlphaBetaPrunning without recursion is fastest for 3x3
            var summaryClassic = BenchmarkRunner.Run<ClassicAI_PerformanceBenchmarkDotNet>();
            //var summaryMulti = BenchmarkRunner.Run<MultiAI_PerformanceBenchmarkDotNet>();
            Console.ReadKey();
        }

        private static void ClassicAI_SimpleTests() {
            SimplePruningTests.General();
            AlphaBetaPrunningTests.General();
            HashingTests.General();

            SimplePruningTests.Time(100);
            AlphaBetaPrunningTests.Time(100);
            HashingTests.Time(100);

            Console.ReadKey();
        }

        private static void MultiAI_SimpleTests() {
            MultiAlphaBetaPrunningTests.General();
            MultiHashingTests.General();

            Console.ReadKey();
        }
    }
}
