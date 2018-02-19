using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

namespace LunchGame01 {
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
        /// <param name="level"></param>
        [UsedImplicitly]
        public void PrepareGameMode(Level level) {
            int m = level.PlayCount % level.NumGameModes;
            gameMode = level.GetGameMode(m);
            gameMode.onGameLost = OnGameLost;
            gameMode.onGameWon = OnGameWon;
            onGameModeDescription?.Invoke(gameMode.title);
        }

        /// <summary>
        /// Handler for onGameReady
        /// </summary>
        [UsedImplicitly]
        public void StartCurrentGameMode() {
            gameMode.Activate();
            hooks.AddOnUpdate(this);
        }

        /// <summary>
        /// Handler for onLevelWillLoad
        /// </summary>
        [UsedImplicitly]
        public void ResetCurrentGameMode() {
            gameMode?.Reset();
        }

        public void UOnUpdate() {
            gameMode.Validate();
        }

        private void OnGameWon() {
            hooks.RemoveOnUpdate(this);
            onGameWon.Invoke();
        }

        private void OnGameLost() {
            hooks.RemoveOnUpdate(this);
            onGameLost.Invoke();
        }
    }
}
