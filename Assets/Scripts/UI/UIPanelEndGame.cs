using System.Collections;
using UnityEngine;

public class UIPanelEndGame : UIPanel {

    [Header("Misc")]
    public ImageColorLerp colorLerp;
    public GameObject     contents;

    public override void Open() {
        base.Open();
        contents.SetActive(false);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        yield return StartCoroutine(colorLerp.LerpColor());
        contents.SetActive(true);
    }

    public override void UOnUpdate() {
        if (Input.GetKeyUp(KeyCode.R)) {
            ui.TriggerNext();
        }
    }
}
