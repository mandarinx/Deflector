using UnityEngine;

namespace Deflector {
    public class Hurt : MonoBehaviour {

        [SerializeField]
        private int value;

        public int GetValue() {
            return value;
        }
    }
}
