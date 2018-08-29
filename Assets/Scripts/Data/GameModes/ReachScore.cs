using GameEvents;
using UnityEngine;

namespace Deflector.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Reach Score")]
    public class ReachScore : GameMode {

        [SerializeField]
        private int                 minScore;
        [SerializeField]
        private IntAsset            scoreAsset;
        [SerializeField]
        private GameEvent           onPlayerDied;
        [SerializeField]
        private GameObjectSet       playerSet;
        [SerializeField]
        private LocalizedTextAsset  description;

        private int                 scoreInit;
        private int                 scoreIncrease;
        private int                 deadPlayers;
        private bool                didWin;

        public override void Validate() {
            // Prioritize game over
            if (deadPlayers >= playerSet.Count) {
                GameLost();
                return;
            }

            if (didWin) {
                return;
            }

            if (scoreIncrease >= minScore) {
                didWin = true;
                GameWon();
            }
        }

        public override string GetDescription(SystemLanguage lang) {
            //Score {score} points
            //Samle {score} poeng
            string desc = description.GetLocalizedText(lang);
            desc = desc.Replace("{score}", minScore.ToString("### ### ##0"));
            return desc;
        }

        public override void Activate() {
            deadPlayers = 0;
            scoreIncrease = 0;
            didWin = false;
            scoreInit = scoreAsset.Value;
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
