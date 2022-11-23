using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests.GameTests
{
    public class GameTests {
        [Fact]
        public void SimulateClassicGame() {
            var game = Factory.CreateNewGame();
            game.StartNewGame(2);

            Assert.Equal(2, game.NextPlayer);

            Assert.True(game.IsMoveLegal(0));
            Assert.True(game.MakeMove(0));

            Assert.False(game.IsMoveLegal(0));
            Assert.False(game.IsGameFinished);

            Assert.True(game.MakeMove(3));
            Assert.True(game.MakeMove(1));
            Assert.True(game.MakeMove(4));
            Assert.True(game.MakeMove(2));
            Assert.Equal(2, game.Winner);

            Assert.True(game.IsGameFinished);
            Assert.Equal(2, game.Winner);

            Assert.True(game.GetWinningCombination(out List<ClassicMove> combination));
            Assert.Contains(new ClassicMove(0, game.Winner), combination);
            Assert.Contains(new ClassicMove(1, game.Winner), combination);
            Assert.Contains(new ClassicMove(2, game.Winner), combination);
        }

        [Fact]
        public void SimulateMultiGame() {
            var game = Factory.CreateNewGame(3,3,2,3,4);
            game.StartNewGame(2);

            Assert.Equal(2, game.NextPlayer);
            Assert.True(game.MakeMoveByPosition(0,0,0));
            Assert.Equal(3, game.NextPlayer);
            Assert.True(game.MakeMoveByPosition(1,1,1));
            Assert.Equal(1, game.NextPlayer);
            Assert.True(game.MakeMoveByPosition(0, 1, 1));
            Assert.Equal(2, game.NextPlayer);

            //p2
            Assert.True(game.MakeMoveByPosition(0, 1, 0));    

            Assert.True(game.MakeMoveByPosition(0, 1, 3));
            Assert.True(game.MakeMoveByPosition(0, 2, 2));

            //p2 winnning
            Assert.True(game.MakeMoveByPosition(0, 2, 0));    

            Assert.True(game.IsGameFinished);
            Assert.Equal(2, game.Winner);

            Assert.True(game.GetWinningCombination(out List<MultiMove> combination));
            Assert.Contains(0, combination.Select(x=>x.Cell.Index));
            Assert.Contains(4, combination.Select(x => x.Cell.Index));
            Assert.Contains(8, combination.Select(x => x.Cell.Index));

        }
    }
}
