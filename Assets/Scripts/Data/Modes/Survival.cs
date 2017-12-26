using GameEvents;
using UnityEngine;

namespace Modes {

    [CreateAssetMenu(menuName = "Game Modes/Survival")]
    public class Survival : GameMode {

        [SerializeField]
        private int             maxProjectiles;
        private int             spawnedProjectiles;
        [SerializeField]
        private GameObjectEvent onProjectileSpawned;

        public override string title => $"Survive {maxProjectiles} projectiles";

        private void OnEnable() {
            spawnedProjectiles = 0;
            onProjectileSpawned?.RegisterCallback(
                go => {
                    ++spawnedProjectiles;
                });
        }

        public override bool Validate() {
            return spawnedProjectiles >= maxProjectiles;
        }
    }
}
