using GameEvents;
using UnityEngine;

namespace Modes {

    [CreateAssetMenu(menuName = "Game Modes/Protect the Treasure")]
    public class ProtectTreasure : GameMode {

        [SerializeField]
        private int             maxProjectiles;
        private int             spawnedProjectiles;
        private int             explodedProjectiles;
        [SerializeField]
        private GameObjectEvent onProjectileSpawned;
        [SerializeField]
        private GameObjectEvent onProjectileExploded;

        public override string title => $"Protect the treasure, and survive {maxProjectiles} projectiles";

        private void OnEnable() {
            spawnedProjectiles = 0;
            onProjectileSpawned?.RegisterCallback(
                go => {
                    ++spawnedProjectiles;
                });
            explodedProjectiles = 0;
            onProjectileExploded?.RegisterCallback(
                go => {
                    ++explodedProjectiles;
                });
        }

        public override bool Validate() {
            return 
                spawnedProjectiles >= maxProjectiles &&
                explodedProjectiles >= maxProjectiles;
        }

        public override void Reset() {
            spawnedProjectiles = 0;
            explodedProjectiles = 0;
        }
    }
}
