using GameEvents;
using UnityEngine;

namespace Modes {

    [CreateAssetMenu(menuName = "Game Modes/Survival")]
    public class Survival : GameMode {

        [SerializeField]
        private int          maxProjectiles;
        private int          spawnedProjectiles;
        [SerializeField]
        private Vector3Event onProjectileSpawned;

        public override string title => $"Survive {maxProjectiles} projectiles";

        private void OnEnable() {
            spawnedProjectiles = 0;
        }

        public override bool Validate() {
            return spawnedProjectiles >= maxProjectiles;
        }

        public override void Activate() {
            spawnedProjectiles = 0;
            onProjectileSpawned?.RegisterCallback(OnProjectileSpawned);
        }

        public override void Reset() {
            spawnedProjectiles = 0;
            onProjectileSpawned?.UnregisterCallback(OnProjectileSpawned);
        }

        private void OnProjectileSpawned(Vector3 pos) {
            ++spawnedProjectiles;
        }
    }
}
