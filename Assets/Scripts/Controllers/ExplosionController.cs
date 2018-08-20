using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {
    public class ExplosionController : MonoBehaviour {

        [SerializeField]
        private GameObject                prefab;
        private GameObjectPool<Explosion> pool;

        private void Awake() {
            pool = new GameObjectPool<Explosion>(transform, prefab, 16, true) {
                OnWillDespawn = e => { e.Despawn(); }
            };
            pool.Fill();
        }

        private IEnumerator Boom(Explosion expl) {
            yield return StartCoroutine(expl.BigBadaBoom());
            pool.Despawn(expl);
        }

        /// <summary>
        /// Called by the event handler for OnProjectileExploded
        /// </summary>
        /// <param name="go"></param>
        [UsedImplicitly]
        public void Spawn(GameObject go) {
            Explosion explosion;
            pool.Spawn(out explosion);
            explosion.transform.position = go.transform.position;
            StartCoroutine(Boom(explosion));
        }

        /// <summary>
        /// Called by the event handler for OnLevelWillLoad
        /// </summary>
        [UsedImplicitly]
        public void DespawnAll() {
            StopAllCoroutines();
            pool.Reset();
        }
    }
}
