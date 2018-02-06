using GameEvents;
using UnityEngine;

namespace Modes {

    [CreateAssetMenu(menuName = "Game Modes/Survival")]
    public class Survival : GameMode {

        [SerializeField]
        private int             maxProjectiles;
        private int             numDespawned;
        [SerializeField]
        private GameObjectEvent onProjectileDespawned;

        public override string title => $"Destroy {maxProjectiles} projectiles";

        private void OnEnable() {
            numDespawned = 0;
        }

        public override bool Validate() {
            return numDespawned >= maxProjectiles;
        }

        public override void Activate() {
            numDespawned = 0;
            onProjectileDespawned?.RegisterCallback(OnProjectileDespawned);
        }

        public override void Reset() {
            numDespawned = 0;
            onProjectileDespawned?.UnregisterCallback(OnProjectileDespawned);
        }

        private void OnProjectileDespawned(GameObject projectile) {
            ++numDespawned;
        }
    }
}
