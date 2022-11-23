using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Game
{
    internal static class ArraysExtenstions
    {
        public static void Fill(this int[,]  array,int value) {
            for(int i =0;i<array.GetLength(0);i++) {
                for(int j=0;j<array.GetLength(1);j++) {
                    array[i, j] =value;
                }

            }
        }
        public static void Fill(this int[] array, int value) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = value;
            }
        }

        public static bool HasZero(this int[] array) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i] == 0) {
                    return true;
                }
            }
            return false;
        }



    }
}
