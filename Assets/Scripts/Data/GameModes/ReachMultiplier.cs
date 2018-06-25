using GameEvents;
using UnityEngine;

namespace Deflector.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Reach Multiplier")]
    public class ReachMultiplier : GameMode {

        [SerializeField]
        private int                minMultipliers;
        [SerializeField]
        private Vector3AndIntEvent onMultiplierIncreasedAt;
        [SerializeField]
        private GameEvent          onPlayerDied;
        [SerializeField]
        private GameObjectSet      playerSet;

        private int                curMultiplier;
        private int                deadPlayers;

        public override string title => $"Reach a {minMultipliers}x multiplier";

        public override void Validate() {
            // Prioritize game over
            if (deadPlayers >= playerSet.Count) {
                GameLost();
                return;
            }

            if (curMultiplier >= minMultipliers) {
                GameWon();
            }
        }

        public override void Activate() {
            deadPlayers = 0;
            curMultiplier = 0;
            onMultiplierIncreasedAt.AddCallback(OnMultiplierIncreasedAt);
            onPlayerDied.RegisterCallback(OnPlayerDied);
        }

        public override void Deactivate() {
            onMultiplierIncreasedAt.RemoveCallback(OnMultiplierIncreasedAt);
            onPlayerDied.UnregisterCallback(OnPlayerDied);
        }

        private void OnMultiplierIncreasedAt(Vector3 pos, int multiplier) {
            curMultiplier = multiplier;
        }

        private void OnPlayerDied() {
            ++deadPlayers;
        }
    }
}
