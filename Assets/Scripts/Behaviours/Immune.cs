using UnityEngine;

namespace LunchGame01 {
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
