using GameEvents;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UHooks))]
public class GameModes : MonoBehaviour, IOnUpdate {
    
    [SerializeField]
    private GameMode  gameMode;
    [SerializeField]
    private GameEvent onGameWon;
    [SerializeField]
    private Text      uiDescription;
    private UHooks    hooks;

    private void Awake() {
        hooks = GetComponent<UHooks>();
    }
    
    // handler for entering intro panel
    public void ShowGameModeDescription() {
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
