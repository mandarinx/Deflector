using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class UIController : MonoBehaviour {

    private readonly Dictionary<string, UIPanel> panels = new Dictionary<string, UIPanel>();
    private Animator   fsm;

    [Header("Events In")]
    public StringEvent onTogglePanelOn;
    public StringEvent onTogglePanelOff;
    
    private void Start() {
        onTogglePanelOn.RegisterCallback(OnTogglePanelOn);
        onTogglePanelOff.RegisterCallback(OnTogglePanelOff);
        
        for (int i = 0; i < transform.childCount; ++i) {
            Transform child = transform.GetChild(i);
            UIPanel panel = child.GetComponent<UIPanel>();
            panel.Init(this);
            panels.Add(child.name, panel);
            OnTogglePanelOff(child.name);
        }

        fsm = GetComponent<Animator>();
        NextState();
    }

    private void OnTogglePanelOn(string panelName) {
        panels[panelName].OnEnter();
    }

    private void OnTogglePanelOff(string panelName) {
        panels[panelName].OnClose();
    }

    public void NextState() {
        fsm.SetTrigger("Next");
    }
    
    // handler for player died event
    public void OnPlayerDied() {
        NextState();
    }
}
