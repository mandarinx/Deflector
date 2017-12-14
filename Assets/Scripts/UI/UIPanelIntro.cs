using GameEvents;
using UnityEngine;

public class UIPanelIntro : UIPanel {

    public GameEvent onEnterGame;
    
    private void Update() {
        if (Input.GetKeyUp(KeyCode.S)) {
            ui.NextState();
            onEnterGame?.Invoke();
        }
    }
}
