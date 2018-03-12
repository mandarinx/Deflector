using UnityEngine;

namespace Deflector {
    public class MinAttribute : PropertyAttribute {

        public readonly float minValue;

        public MinAttribute(float value) {
            minValue = value;
        }
    }
}
