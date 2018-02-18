using System.Collections;
using UnityEngine;

namespace LunchGame01 {
    public class ExplosionController : MonoBehaviour {

        [SerializeField]
        private GameObject                prefab;
        private GameObjectPool<Explosion> pool;

        private void Awake() {
            pool = new GameObjectPool<Explosion>(transform, prefab, 16, true) {
                OnSpawned = OnExplosionSpawned,
                OnWillDespawn = e => { e.StopAllCoroutines(); }
            };
            pool.Fill();
        }

        private void OnExplosionSpawned(Explosion expl) {
            StartCoroutine(Boom(expl));
        }

        private IEnumerator Boom(Explosion expl) {
            yield return StartCoroutine(expl.BigBadaBoom());
            pool.Despawn(expl);
        }

        public void Spawn(GameObject go) {
            Explosion explosion;
            pool.Spawn(out explosion);
            explosion.transform.position = go.transform.position;
        }

        public void DespawnAll() {
            pool.Reset();
        }
    }
}
