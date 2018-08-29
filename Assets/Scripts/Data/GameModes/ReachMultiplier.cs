using GameEvents;
using UnityEngine;

namespace Deflector.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Reach Multiplier")]
    public class ReachMultiplier : GameMode {

        [SerializeField]
        private int                 minMultipliers;
        [SerializeField]
        private Vector3AndIntEvent  onMultiplierIncreasedAt;
        [SerializeField]
        private GameEvent           onPlayerDied;
        [SerializeField]
        private GameObjectSet       playerSet;
        [SerializeField]
        private LocalizedTextAsset  description;

        private int                 curMultiplier;
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

            if (curMultiplier >= minMultipliers) {
                didWin = true;
                GameWon();
            }
        }

        public override string GetDescription(SystemLanguage lang) {
            string desc = description.GetLocalizedText(lang);
            desc = desc.Replace("{multipliers}", minMultipliers.ToString());
            return desc;
        }

        public override void Activate() {
            deadPlayers = 0;
            curMultiplier = 0;
            didWin = false;
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
