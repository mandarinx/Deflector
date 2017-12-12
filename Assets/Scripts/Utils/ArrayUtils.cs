using UnityEngine;

namespace HyperGames.Lib {

    public static class ArrayUtils {
    
        public static void Randomize(int[] list) {
            // Randomize using Fisher-Yates algorithm
            for (int i = list.Length; i > 1; --i) {
                int j = Random.Range(0, (i - 1));
                int tmp = list[j];
                list[j] = list[i - 1];
                list[i - 1] = tmp;
            }
        }

        public static void FillSequence(int[] list) {
            for (int i = 0; i < list.Length; ++i) {
                list[i] = i;
            }
        }
    }
}
