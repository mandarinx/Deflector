using GameEvents;
using UnityEngine;

namespace LunchGame01.Modes {
    [CreateAssetMenu(menuName = "Game Modes/Chain Reaction")]
    public class ChainReaction : GameMode {

        [SerializeField]
        private int           minChainReactions;
        [SerializeField]
        private GameEvent     onChainReaction;
        [SerializeField]
        private GameEvent     onPlayerDied;
        [SerializeField]
        private GameObjectSet playerSet;

        private int           numChainReactions;
        private int           deadPlayers;

        public override string title => $"Cause {minChainReactions} chain reactions of explosions";

        public override void Validate() {
            // Prioritize game over
            if (deadPlayers >= playerSet.Count) {
                GameLost();
                return;
            }

            if (numChainReactions >= minChainReactions) {
                GameWon();
            }
        }

        public override void Activate() {
            deadPlayers = 0;
            numChainReactions = 0;
            onChainReaction.RegisterCallback(OnChainReaction);
            onPlayerDied.RegisterCallback(OnPlayerDied);
        }

        public override void Reset() {
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
