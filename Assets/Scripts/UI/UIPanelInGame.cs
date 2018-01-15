using System.Collections;
using GameEvents;
using UnityEngine;

public class UIPanelInGame : UIPanel {

    public GameEvent    onGameReady;
    
    [Header("Misc")]
    public PlayerHearts hearts;
    
    public override void Open() {
        base.Open();
        StartCoroutine(TransitionEnter());
    }

    private IEnumerator TransitionEnter() {
        yield return StartCoroutine(hearts.RenderHearts());
        onGameReady?.Invoke();
    }
}
