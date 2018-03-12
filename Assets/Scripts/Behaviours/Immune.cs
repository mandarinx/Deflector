using UnityEngine;

namespace Deflector {
    [RequireComponent(typeof(Player))]
    public class Immune : MonoBehaviour {

        private void Update() {
            GetComponent<Player>().SetImmune(true);
        }

        private void OnDisable() {
            GetComponent<Player>().SetImmune(false);
        }
    }
}
