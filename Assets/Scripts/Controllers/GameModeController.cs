using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {
    public class GameModeController : MonoBehaviour, IOnUpdate {

        [SerializeField]
        private GameEvent   onGameWon;
        [SerializeField]
        private GameEvent   onGameLost;
        [SerializeField]
        private StringEvent onGameModeDescription;
        [SerializeField]
        private UHooks      hooks;

        private GameMode    gameMode;

        /// <summary>
        /// Handler for onLevelLoaded
        /// </summary>
        [UsedImplicitly]
        public void PrepareGameMode(Level level) {
            gameMode = level.GameMode;
            gameMode.onGameLost = OnGameLost;
            gameMode.onGameWon = OnGameWon;
            onGameModeDescription.Invoke(gameMode.title);
        }

        /// <summary>
        /// Handler for onGameReady
        /// </summary>
        [UsedImplicitly]
        public void ActivateGameMode() {
            gameMode.Activate();
            hooks.AddOnUpdate(this);
        }

        public void UOnUpdate() {
            gameMode.Validate();
        }

        private void OnGameWon() {
            hooks.RemoveOnUpdate(this);
            onGameWon.Invoke();
            gameMode.Deactivate();
        }

        private void OnGameLost() {
            hooks.RemoveOnUpdate(this);
            onGameLost.Invoke();
            gameMode.Deactivate();
        }
    }
}
