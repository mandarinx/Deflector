using System.Collections;
using GameEvents;
using UnityEngine;

public class UIPanelInGame : UIPanel {

    public GameEvent            onGameReady;
    
    [Header("Misc")]
    public PlayerHealthListener healthListener;
    
    public override void OnEnter() {
        base.OnEnter();
        StartCoroutine(TransitionEnter());
    }

    private IEnumerator TransitionEnter() {
        yield return StartCoroutine(healthListener.RenderHearts());
        onGameReady.Invoke();
    }
}
