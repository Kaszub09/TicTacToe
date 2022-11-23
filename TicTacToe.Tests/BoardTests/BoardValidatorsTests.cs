using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests.BoardTests {
    public class BoardValidatorsTests {
        [Fact]
        public void TestClassicBoardValidation() {
            var cBoard = new MultiBoard(new Space(3, 3));

            var cbValidator = new ClassicBoardValidator(cBoard.GetGridReference());
            var mbValidator = new MultiBoardValidator(cBoard.GetGridReference(),cBoard.Space,3);

            var cWins = cbValidator.PossibleWins.Select(x=> { var l = x.ToList(); l.Sort(); return l; }).ToList();
            cWins.Sort(Comparelists);
            var mWins = mbValidator.PossibleWins.Select(x => { var l = x.ToList(); l.Sort(); return l; }).ToList();
            mWins.Sort(Comparelists);

            Assert.Equal(cWins,mWins);
        }

        [Fact]
        public void Test3Dvalidation() {
            var cBoard = new MultiBoard(new Space(1,1,2));
            var mbValidator = new MultiBoardValidator(cBoard.GetGridReference(), cBoard.Space, 2);

            Assert.Equal(new List<int[]>() { new int[] { 0, 1 } }, mbValidator.PossibleWins);
        }

        public int Comparelists(List<int> l1, List<int> l2) {
            if (l1.Count != l2.Count) return l1.Count.CompareTo(l2.Count);

            for (int i = 0; i < l1.Count; i++) {
                if (l1[i] != l2[i]) return l1[i].CompareTo(l2[i]);
            }

            return 0;
        }

    }
}
