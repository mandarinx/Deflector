using GameEvents;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIPanel : MonoBehaviour {
    
    [Header("Events Out")]
    public GameEvent       onEnterPanel;
    public GameEvent       onClosePanel;
    
    protected UIController ui;

    private Canvas         canvas;

    private void Awake() {
        canvas = GetComponent<Canvas>();
    }

    public void Init(UIController uiController) {
        ui = uiController;
    }

    public void Hide() {
        canvas.enabled = false;
    }

    public void Show() {
        canvas.enabled = true;
    }
    
    public virtual void OnEnter() {
        Show();
        onEnterPanel?.Invoke();
    }

    public virtual void OnClose() {
        Hide();
        onClosePanel?.Invoke();
    }
}
