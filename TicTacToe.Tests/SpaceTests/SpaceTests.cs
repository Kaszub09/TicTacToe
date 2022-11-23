using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests.SpaceTests {
    public class SpaceTests {
        [Fact]
        public void IsInSpace() {
            var space = new Space(3,4,5);

            Assert.Equal(3, space.Dimensions);

            Assert.True(space.IsIndexInSpace(0));
            Assert.True(space.IsIndexInSpace(space.SpaceSize-1));
            Assert.False(space.IsIndexInSpace(-1));
            Assert.False(space.IsIndexInSpace(space.SpaceSize));


            Assert.True(space.IsPositionInSpace(1,2,4));
            Assert.True(space.IsPositionInSpace(0, 0, 0));
            Assert.True(space.IsPositionInSpace(2, 3, 5));
            Assert.False(space.IsPositionInSpace(1,-1,2));
            Assert.False(space.IsPositionInSpace(1,5,3));
        }

        [Fact]
        public void PositionConversion() {
            var space = new Space(3, 4, 5);

            Assert.Equal(0, space.GetIndex(new int[] { 0, 0, 0 }));
            Assert.Equal(3*1, space.GetIndex(new int[] {0,0,3}));
            Assert.Equal(1*4*5+2*5+2, space.GetIndex(new int[] { 1, 2, 2 }));
            Assert.Equal(space.SpaceSize-1, space.GetIndex(new int[] { 2,3,4}));


            Assert.Equal(new int[] { 0, 0, 0 }, space.GetPosition(0));
            Assert.Equal(new int[] { 0, 0, 3 }, space.GetPosition(3 * 1));
            Assert.Equal(new int[] { 1, 2, 2 }, space.GetPosition(1 * 4 * 5 + 2 * 5 + 2));
            Assert.Equal(new int[] { 2, 3, 4 }, space.GetPosition(space.SpaceSize - 1));

        }
    }
}
