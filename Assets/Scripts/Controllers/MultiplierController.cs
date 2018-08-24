using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {
    public class MultiplierController : MonoBehaviour {

        [SerializeField]
        private GameObject                 prefab;
        [SerializeField]
        private Transform                  parent;

        private GameObjectPool<Multiplier> pool;

        private void Awake() {
            pool = new GameObjectPool<Multiplier>(parent, prefab, 64, true);
            pool.Fill();
        }

        /// <summary>
        /// Called by the event handler for OnMultiplierWillDespawn
        /// </summary>
        /// <param name="projectile"></param>
        [UsedImplicitly]
        public void Despawn(GameObject projectile) {
            pool.Despawn(projectile.GetComponent<Multiplier>());
        }

        /// <summary>
        /// Called by the event handler for OnLevelWillLoad and OnLevelExit
        /// </summary>
        [UsedImplicitly]
        public void DespawnAll() {
            pool.Reset();
        }

        /// <summary>
        /// Called by the event handler for OnMultiplierIncreasedAt
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value"></param>
        [UsedImplicitly]
        public void Spawn(Vector3 position, int value) {
            Multiplier multiplier;
            pool.Spawn(out multiplier);
            multiplier.Activate(position, value);
        }
    }
}
