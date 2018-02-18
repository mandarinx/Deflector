using UnityEngine;

namespace LunchGame01 {
    public class Hitable : MonoBehaviour {

        [SerializeField]
        private UnityIntEvent onHit;

        public void Hit(int hitAngle) {
            onHit.Invoke(hitAngle);
        }
    }
}
