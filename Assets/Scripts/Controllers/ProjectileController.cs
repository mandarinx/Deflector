using UnityEngine;

namespace Deflector {
    public class ProjectileController : MonoBehaviour {

        [SerializeField]
        private GameObject                 prefab;
        private GameObjectPool<Projectile> pool;

        private void Awake() {
            pool = new GameObjectPool<Projectile>(transform, prefab, 16, true) {
                OnWillSpawn = p => { p.tag = "Untagged"; }
            };
            pool.Fill();
        }

        public void Despawn(GameObject projectile) {
            pool.Despawn(projectile.GetComponent<Projectile>());
        }

        public void Spawn(Vector3 pos) {
            Projectile projectile;
            pool.Spawn(out projectile);
            projectile.transform.position = pos;
        }

        public void DespawnAll() {
            pool.Reset();
        }
    }
}
