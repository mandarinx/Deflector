using GameEvents;
using UnityEngine;

namespace Deflector.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Reach Score")]
    public class ReachScore : GameMode {

        [SerializeField]
        private int            minScore;
        [SerializeField]
        private IntAsset       scoreAsset;
        [SerializeField]
        private GameEvent      onPlayerDied;
        [SerializeField]
        private GameObjectSet  playerSet;

        private int            scoreInit;
        private int            scoreIncrease;
        private int            deadPlayers;

        public override string title => $"Score {minScore:### ### ##0} points";

        public override void Validate() {
            // Prioritize game over
            if (deadPlayers >= playerSet.Count) {
                GameLost();
                return;
            }

            if (scoreIncrease >= minScore) {
                GameWon();
            }
        }

        public override void Activate() {
            deadPlayers = 0;
            scoreIncrease = 0;
            scoreInit = scoreAsset.value;
            scoreAsset.AddChangeCallback(OnScoreChanged);
            onPlayerDied.RegisterCallback(OnPlayerDied);
        }

        public override void Deactivate() {
            scoreAsset.RemoveChangeCallback(OnScoreChanged);
            onPlayerDied.UnregisterCallback(OnPlayerDied);
        }

        private void OnScoreChanged(int score) {
            scoreIncrease = score - scoreInit;
        }

        private void OnPlayerDied() {
            ++deadPlayers;
        }
    }
}
