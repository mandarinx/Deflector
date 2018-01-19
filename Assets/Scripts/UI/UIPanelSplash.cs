using UnityEngine;

public class UIPanelSplash : UIPanel {

    public override void UOnUpdate() {
        if (Input.GetKeyUp(KeyCode.S)) {
            ui.TriggerNext();
        }
        if (Input.GetKeyUp(KeyCode.C)) {
            ui.TriggerCredits();
        }
    }
}
