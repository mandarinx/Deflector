using GameEvents;
using UnityEngine;

namespace Deflector.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Destroy Projectile")]
    public class DestroyProjectile : GameMode {

        [SerializeField]
        private int                 maxProjectiles;
        private int                 numDespawned;
        [SerializeField]
        private GameObjectEvent     onProjectileDespawned;
        [SerializeField]
        private GameEvent           onPlayerDied;
        [SerializeField]
        private GameObjectSet       playerSet;
        [SerializeField]
        private LocalizedTextAsset  description;

        private int                 deadPlayers;
        private bool                didWin;

        public override void Validate() {
            if (deadPlayers >= playerSet.Count) {
                GameLost();
                return;
            }

            if (didWin) {
                return;
            }

            if (numDespawned >= maxProjectiles) {
                didWin = true;
                GameWon();
            }
        }

        public override string GetDescription(SystemLanguage lang) {
            string desc = description.GetLocalizedText(lang);
            desc = desc.Replace("{projectiles}", maxProjectiles.ToString());
            return desc;
        }

        public override void Activate() {
            numDespawned = 0;
            deadPlayers = 0;
            didWin = false;
            onProjectileDespawned.RegisterCallback(OnProjectileDespawned);
            onPlayerDied.RegisterCallback(OnPlayerDied);
        }

        public override void Deactivate() {
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
