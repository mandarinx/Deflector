using GameEvents;
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

        public void PrepareGameMode(Level level) {
            int r = Random.Range(0, level.NumGameModes);
            gameMode = level.GetGameMode(r);
            gameMode.onGameLost = OnGameLost;
            gameMode.onGameWon = OnGameWon;
            onGameModeDescription?.Invoke(gameMode.title);
        }

        public void StartCurrentGameMode() {
            gameMode.Activate();
            hooks.AddOnUpdate(this);
        }

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
