using GameEvents;
using UnityEngine;

namespace Deflector.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Chain Reaction")]
    public class ChainReaction : GameMode {

        [SerializeField]
        private int                 minChainReactions;
        [SerializeField]
        private GameEvent           onChainReaction;
        [SerializeField]
        private GameEvent           onPlayerDied;
        [SerializeField]
        private GameObjectSet       playerSet;
        [SerializeField]
        private LocalizedTextAsset  description;

        private int                 numChainReactions;
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

            if (numChainReactions >= minChainReactions) {
                didWin = true;
                GameWon();
            }
        }

        public override string GetDescription(SystemLanguage lang) {
            //Cause a chain reaction of {chain_reactions} explosions
            //Lag en kjedereaksjon med {chain_reactions} eksplosjoner
            string desc = description.GetLocalizedText(lang);
            desc = desc.Replace("{chain_reactions}", minChainReactions.ToString());
            return desc;
        }

        public override void Activate() {
            deadPlayers = 0;
            numChainReactions = 0;
            didWin = false;
            onChainReaction.RegisterCallback(OnChainReaction);
            onPlayerDied.RegisterCallback(OnPlayerDied);
        }

        public override void Deactivate() {
            onChainReaction.UnregisterCallback(OnChainReaction);
            onPlayerDied.UnregisterCallback(OnPlayerDied);
        }

        private void OnChainReaction() {
            ++numChainReactions;
        }

        private void OnPlayerDied() {
            ++deadPlayers;
        }
    }
}
