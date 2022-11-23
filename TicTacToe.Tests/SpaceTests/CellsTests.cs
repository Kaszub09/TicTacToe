using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests.SpaceTests {
    public class CellsTests {
        [Fact]
        public void DimCorrectnes() {
            var space = new Space(3, 4, 5);

            var cell = new MultiCell(space,1,2,3);

            Assert.Equal(1 * 4 * 5 + 2 * 5 + 3, cell.Index);
            Assert.Equal(1 , cell[0]);
            Assert.Equal(2, cell[1]);
            Assert.Equal(3, cell[2]);

             cell = new MultiCell(space, 1 * 4 * 5 + 2 * 5 + 3);

            Assert.Equal(new int[] { 1, 2, 3 }, cell.Position);
            Assert.Equal(1, cell[0]);
            Assert.Equal(2, cell[1]);
            Assert.Equal(3, cell[2]);

        }
    }
}
