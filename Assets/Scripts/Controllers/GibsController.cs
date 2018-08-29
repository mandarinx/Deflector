using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {
    public class GibsController : MonoBehaviour {

        [SerializeField]
        private ParticleSystem                 prefab;

        private GameObjectPool<ParticleSystem> pool;

        private void Awake() {
            pool = new GameObjectPool<ParticleSystem>(parent: transform,
                                                      prefab: prefab.gameObject,
                                                      size:   2,
                                                      grow:   false);
            pool.Fill();
        }

        [UsedImplicitly]
        public void SpawnAt(Vector3 position) {
            ParticleSystem psys;
            pool.Spawn(out psys);
            psys.transform.position = position;
            psys.Play();
            StartCoroutine(DespawnWhenDone(psys, pool));
        }

        [UsedImplicitly]
        public void DespawnAll() {
            pool.Reset();
        }

        private static IEnumerator DespawnWhenDone(ParticleSystem                 psys,
                                                   GameObjectPool<ParticleSystem> psysPool) {
            while (psys.isPlaying) {
                yield return null;
            }
            psysPool.Despawn(psys);
        }
    }
}
