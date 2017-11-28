using System.Collections;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class UIIngamePanel : MonoBehaviour {
    
    [Tooltip("Triggered when all InGame animations are done")]
    public GameEvent            panelReadyEvent;
    public PlayerHealthListener healthListener;
    
    public void EnterPanel() {
        StartCoroutine(TransitionEnter());
    }

    private IEnumerator TransitionEnter() {
        yield return StartCoroutine(healthListener.LoadHearts());
        panelReadyEvent.Raise();
    }
}
