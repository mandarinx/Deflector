using UnityEngine;

namespace Deflector {

    public class CheatController : MonoBehaviour {

        public ProjectileController projectiles;

        private void Update() {
            if (Input.GetKeyUp(KeyCode.V)) {
                foreach (Transform child in projectiles.transform) {
                    Projectile p = child.gameObject.GetComponent<Projectile>();
                    if (p == null) {
                        continue;
                    }

                    if (!p.gameObject.activeSelf) {
                        continue;
                    }

                    p.Explode(Vector3.zero);
                }
            }
            if (Input.GetKeyUp(KeyCode.C)) {
                foreach (Transform child in projectiles.transform) {
                    Projectile p = child.gameObject.GetComponent<Projectile>();
                    if (p == null) {
                        continue;
                    }

                    if (!p.gameObject.activeSelf) {
                        continue;
                    }

                    if (p.IsCharged || p.IsExploding) {
                        continue;
                    }

                    p.Hit(Random.Range(0,4));
                }
            }
            if (Input.GetKeyUp(KeyCode.B)) {
                foreach (Transform child in projectiles.transform) {
                    Projectile p = child.gameObject.GetComponent<Projectile>();
                    if (p == null) {
                        continue;
                    }

                    if (!p.gameObject.activeSelf) {
                        continue;
                    }

                    if (p.IsCharged) {
                        p.Hit(Random.Range(0,4));
                    }
                }
            }
        }
    }
}
