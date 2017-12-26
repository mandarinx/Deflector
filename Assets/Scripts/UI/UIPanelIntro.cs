using GameEvents;
using UnityEngine;

public class UIPanelIntro : UIPanel {

    public GameEvent onEnterGame;

    public override void UOnUpdate() {
        if (Input.GetKeyUp(KeyCode.S)) {
            ui.NextState();
            onEnterGame?.Invoke();
        }
    }
}
