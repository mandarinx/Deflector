using UnityEngine;

namespace LunchGame01 {
    public class Hurt : MonoBehaviour {

        [SerializeField]
        private int value;

        public int GetValue() {
            return value;
        }
    }
}
