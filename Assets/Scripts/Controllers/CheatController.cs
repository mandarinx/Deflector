using UnityEngine;

namespace Deflector {

    public class CheatController : MonoBehaviour {

        public Transform projectiles;
        public Transform players;
        public IntAsset score;
        public HealthAsset health;

        private void Update() {
            if (Input.GetKeyUp(KeyCode.N)) {
                score.SetValue(Random.Range(0, 25) * 250000);
            }
            if (Input.GetKeyUp(KeyCode.M)) {
                health.SetLives(3);
            }
            if (Input.GetKeyUp(KeyCode.V)) {
                foreach (Transform child in projectiles) {
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
                foreach (Transform child in projectiles) {
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
                foreach (Transform child in projectiles) {
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
            if (Input.GetKeyUp(KeyCode.I)) {
                foreach (Transform child in players) {
                    Immune i = child.gameObject.GetComponent<Immune>();
                    if (i == null) {
                        continue;
                    }

                    i.enabled = !i.enabled;
                    Debug.Log($"{child.name} {(i.enabled ? "immune" : "no longer immune")}");
                }
            }
        }
    }
}
