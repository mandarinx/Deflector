using System.Collections;
using UnityEngine;

namespace LunchGame01 {
    public class TurretController : MonoBehaviour {

        [SerializeField]
        private float                  spawnInterval = 4;
        [SerializeField]
        private int                    maxTurrets = -1;
        [SerializeField]
        private SpawnPointSet          spawnPoints;
        [SerializeField]
        private GameObject             prefab;
        private GameObjectPool<Turret> pool;

        private void Awake() {
            pool = new GameObjectPool<Turret>(transform, prefab, 32, true);
            pool.Fill();
        }

        public void Activate() {
            StartCoroutine(Spawn());
        }

        public void Deactivate() {
            StopAllCoroutines();
            DespawnAll();
        }

        public void Despawn(GameObject projectile) {
            pool.Despawn(projectile.GetComponent<Turret>());
        }

        public void Spawn(Vector3 pos) {
            Turret turret;
            pool.Spawn(out turret);
            turret.transform.position = pos;
        }

        public void DespawnAll() {
            pool.Reset();
        }

        private IEnumerator Spawn() {
            while (maxTurrets < 0 || pool.NumSpawned < maxTurrets) {
                yield return new WaitForSeconds(spawnInterval);
                if (spawnPoints.Count < 1) {
                    continue;
                }
                Turret turret;
                pool.Spawn(out turret);
                turret.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
            }
        }
    }
}
