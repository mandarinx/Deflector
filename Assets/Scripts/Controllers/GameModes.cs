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
        int r = Random.Range(0, level.NumGameModes);
        gameMode = level.GetGameMode(r);
        uiDescription.text = gameMode.title;
    }

    // handler for game ready event
    public void StartGameMode() {
        hooks.AddOnUpdate(this);
    }

    public void ResetCurrentGameMode() {
        gameMode?.Reset();
    }

    public void UOnUpdate() {
        if (!gameMode.Validate()) {
            return;
        }
        onGameWon.Invoke();
        hooks.RemoveOnUpdate(this);
    }
}
