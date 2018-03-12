using GameEvents;
using UnityEngine;

namespace Deflector.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Destroy Projectile")]
    public class DestroyProjectile : GameMode {

        [SerializeField]
        private int             maxProjectiles;
        private int             numDespawned;
        [SerializeField]
        private GameObjectEvent onProjectileDespawned;
        [SerializeField]
        private GameEvent       onPlayerDied;
        [SerializeField]
        private GameObjectSet   playerSet;

        private int             deadPlayers;

        public override string title => $"Destroy {maxProjectiles} "+
                                        $"projectile{(maxProjectiles > 1 ? "s" : "")}";

        public override void Validate() {
            // Prioritize game over
            if (deadPlayers >= playerSet.Count) {
                GameLost();
                return;
            }

            if (numDespawned >= maxProjectiles) {
                GameWon();
            }
        }

        public override void Activate() {
            numDespawned = 0;
            deadPlayers = 0;
            onProjectileDespawned.RegisterCallback(OnProjectileDespawned);
            onPlayerDied.RegisterCallback(OnPlayerDied);
        }

        public override void Reset() {
            numDespawned = 0;
            deadPlayers = 0;
            onProjectileDespawned.UnregisterCallback(OnProjectileDespawned);
            onPlayerDied.UnregisterCallback(OnPlayerDied);
        }

        private void OnPlayerDied() {
            ++deadPlayers;
        }

        private void OnProjectileDespawned(GameObject projectile) {
            ++numDespawned;
        }
    }
}
