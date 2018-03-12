using UnityEngine;

namespace Deflector {
    public class Hitable : MonoBehaviour {

        [SerializeField]
        private UnityIntEvent onHit;

        public void Hit(int hitAngle) {
            onHit.Invoke(hitAngle);
        }
    }
}
