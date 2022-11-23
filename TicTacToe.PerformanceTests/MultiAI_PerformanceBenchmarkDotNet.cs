using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI;
using TicTacToe.Game;

namespace TicTacToe.PerformanceTests {
    [MemoryDiagnoser]
    [SimpleJob(launchCount: 1, warmupCount: 0, targetCount: 1)]
    public class MultiAI_PerformanceBenchmarkDotNet {
        Dictionary<string, MultiAI> _ai;
        IMultiGame _game;

        [GlobalSetup]
        public void PuzzleSetup() {
            _ai = new Dictionary<string, MultiAI>();
            _ai["hashing"] = new MultiAI_Hashing();
            _ai["alphaBeta"] = new MultiAI_AlphaBetaPrunning();
            _game = Factory.CreateNewGame(2, 4,3,4);
        }

        [Benchmark]
        [Arguments("hashing")]
        [Arguments("alphaBeta")]
        public MultiMoveEval BestMoveNoRec(string aiName) {
            return _ai[aiName].GetBestMoveNoRecVal(_game);
        }
        [Benchmark]
        [Arguments("hashing")]
        [Arguments("alphaBeta")]
        public MultiMoveEval BestMoveRec(string aiName) {
            return _ai[aiName].GetBestMoveRecVal(_game);
        }
    }
}