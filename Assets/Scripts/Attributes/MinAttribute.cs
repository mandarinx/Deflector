using UnityEngine;

namespace LunchGame01 {
    public class MinAttribute : PropertyAttribute {

        public readonly float minValue;

        public MinAttribute(float value) {
            minValue = value;
        }
    }
}
