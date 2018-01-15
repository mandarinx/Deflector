using GameEvents;
using UnityEngine;
using UnityEngine.UI;

public class GameModes : MonoBehaviour, IOnUpdate {
    
    [SerializeField]
    private GameEvent onGameWon;
    [SerializeField]
    private Text      uiDescription;
    [SerializeField]
    private UHooks    hooks;
    private GameMode  gameMode;

    public void OnLevelLoaded(Level level) {
        gameMode = level.mode;
        uiDescription.text = gameMode.title;
    }

    // handler for game ready event
    public void StartGameMode() {
        hooks.AddOnUpdate(this);
    }

    public void UOnUpdate() {
        if (!gameMode.Validate()) {
            return;
        }
        onGameWon.Invoke();
        hooks.RemoveOnUpdate(this);
    }
}
