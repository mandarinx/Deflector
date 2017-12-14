using GameEvents;
using UnityEngine;

[RequireComponent(typeof(UHooks))]
public class GameModes : MonoBehaviour, IOnUpdate {
    
    [SerializeField]
    private GameMode  gameMode;
    [SerializeField]
    private GameEvent onWin;
    private UHooks    hooks;

    private void Awake() {
        hooks = GetComponent<UHooks>();
    }

    // handler for game ready event
    public void StartGameMode() {
        hooks.AddOnUpdate(this);
    }

    public void UOnUpdate() {
        if (gameMode.Validate()) {
            onWin.Invoke();
            hooks.RemoveOnUpdate(this);
        }
    }
}
