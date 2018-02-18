using UnityEngine;

namespace LunchGame01 {
    public class Killable : MonoBehaviour {

        public UnityVector3Event onKilled;

        public void Kill(Vector3 hitPos) {
            onKilled.Invoke(hitPos);
        }
    }
}
