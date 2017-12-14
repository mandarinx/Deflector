using System.Collections;
using GameEvents;
using UnityEngine;

public class UIPanelEndGame : UIPanel, IOnUpdate {

    public GameEvent      onGameReset;
    
    [Header("Misc")]
    public UHooks         uhooks;
    public ImageColorLerp colorLerp;
    public GameObject     contents;

    public override void OnEnter() {
        contents.SetActive(false);
        Show();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        yield return StartCoroutine(colorLerp.LerpColor());
        onEnterPanel?.Invoke();
        contents.SetActive(true);
        uhooks.AddOnUpdate(this);
    }

    public void UOnUpdate() {
        if (Input.GetKeyUp(KeyCode.R)) {
            onGameReset.Invoke();
            ui.NextState();
            uhooks.RemoveOnUpdate(this);
        }
    }
}
