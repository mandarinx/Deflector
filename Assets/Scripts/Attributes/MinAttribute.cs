using UnityEngine;

namespace HyperGames.Lib {

    public class MinAttribute : PropertyAttribute {
    
        public readonly float minValue;
    
        public MinAttribute(float value) {
            minValue = value;
        }
    }
}
