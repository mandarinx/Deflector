using UnityEngine;
using System.Collections.Generic;

namespace Deflector {
    public static class Angles {

        private static readonly Dictionary<int, float> radianMap = new Dictionary<int, float> {
            { 0, Mathf.PI *  0f },
            { 1, Mathf.PI *  0.25f },
            { 2, Mathf.PI *  0.5f },
            { 3, Mathf.PI *  0.75f },
            { 4, Mathf.PI *  1f },
            { 5, Mathf.PI * -0.75f },
            { 6, Mathf.PI * -0.5f },
            { 7, Mathf.PI * -0.25f },
        };

        private static readonly Dictionary<int, int> angleMap = new Dictionary<int, int> {
            { Mathf.FloorToInt(Mathf.PI *  0f    * 1000f), 0 },
            { Mathf.FloorToInt(Mathf.PI *  0.25f * 1000f), 1 },
            { Mathf.FloorToInt(Mathf.PI *  0.5f  * 1000f), 2 },
            { Mathf.FloorToInt(Mathf.PI *  0.75f * 1000f), 3 },
            { Mathf.FloorToInt(Mathf.PI *  1f    * 1000f), 4 },
            { Mathf.FloorToInt(Mathf.PI * -0.75f * 1000f), 5 },
            { Mathf.FloorToInt(Mathf.PI * -0.5f  * 1000f), 6 },
            { Mathf.FloorToInt(Mathf.PI * -0.25f * 1000f), 7 },
        };

        public static float GetAngle(int index) {
            return radianMap[index];
        }

        public static Vector2 GetDirection(int angleIndex) {
            float angle = radianMap[angleIndex];
            return new Vector2 {
                x = Mathf.Cos(angle),
                y = Mathf.Sin(angle)
            };
        }

        public static Vector2 GetDirection(float angle) {
            return new Vector2 {
                x = Mathf.Cos(angle),
                y = Mathf.Sin(angle)
            };
        }

        public static float GetRadian(Vector2 vec) {
            return Mathf.Atan2(vec.y, vec.x);
        }

        public static int GetAngleIndex(float radian) {
            int index;
            return angleMap.TryGetValue(Mathf.FloorToInt(radian * 1000f), out index) ? index : -1;
        }
    }
}
