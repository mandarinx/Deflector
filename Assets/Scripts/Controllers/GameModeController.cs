using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {
    public class GameModeController : MonoBehaviour, IOnUpdate {

        [SerializeField]
        private GameEvent       onGameWon;
        [SerializeField]
        private GameEvent       onGameLost;
        [SerializeField]
        private GameEvent       onGameModeWinCoditionMet;
        [SerializeField]
        private StringEvent     onGameModeDescription;
        [SerializeField]
        private UHooks          hooks;

        private GameMode        gameMode;
        private SystemLanguage  curLang;

        /// <summary>
        /// Handler for onLevelLoaded
        /// </summary>
        [UsedImplicitly]
        public void PrepareGameMode(Level level) {
            gameMode = level.GameMode;
            gameMode.onGameLost = OnGameLost;
            gameMode.onGameWon = OnGameWon;
            onGameModeDescription.Invoke(gameMode.GetDescription(curLang));
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

        /// <summary>
        /// Handler for onLevelExit
        /// </summary>
        [UsedImplicitly]
        public void OnLevelExit() {
            hooks.RemoveOnUpdate(this);
            onGameWon.Invoke();
            gameMode.Deactivate();
        }

        public void OnLanguageChanged(int langId) {
            curLang = (SystemLanguage) langId;
        }

        private void OnGameWon() {
            onGameModeWinCoditionMet.Invoke();
        }

        private void OnGameLost() {
            hooks.RemoveOnUpdate(this);
            onGameLost.Invoke();
            gameMode.Deactivate();
        }
    }
}
