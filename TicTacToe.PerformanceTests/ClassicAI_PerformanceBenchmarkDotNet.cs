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
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 30)]
    public class ClassicAI_PerformanceBenchmarkDotNet {
        Dictionary<string, ClassicAI> _ai = new Dictionary<string, ClassicAI>();
        IClassicGame _game = Factory.CreateNewGame();
        [GlobalSetup]
        public void PuzzleSetup() {
            _ai = new Dictionary<string, ClassicAI>();
            _ai["simple"] = new ClassicAI_SimplePrunning();
            _ai["hashing"] = new ClassicAI_Hashing();
            _ai["alphaBeta"] = new ClassicAI_AlphaBetaPrunning();
            _game = Factory.CreateNewGame();
        }

        [Benchmark]
        [Arguments("simple")]
        [Arguments("hashing")]
        [Arguments("alphaBeta")]
        public ClassicMoveEval BestMoveNoRec(string aiName) {
            return _ai[aiName].GetBestMoveNoRecVal(_game.Board, 1);
        }
        [Benchmark]
        [Arguments("simple")]
        [Arguments("hashing")]
        [Arguments("alphaBeta")]
        public ClassicMoveEval BestMoveRec(string aiName) {
            return _ai[aiName].GetBestMoveRecVal(_game.Board, 1);
        }
        [Benchmark]
        [Arguments("simple")]
        [Arguments("hashing")]
        [Arguments("alphaBeta")]
        public List<ClassicMoveEval> AllBestMoves(string aiName) {
            return _ai[aiName].GetAllBestMoves(_game.Board, 1);
        }

        [Benchmark]
        [Arguments("simple")]
        [Arguments("hashing")]
        [Arguments("alphaBeta")]
        public List<ClassicMoveEval> AllMoves(string aiName) {
            return _ai[aiName].GetAllMoves(_game.Board, 1);
        }

    }
}