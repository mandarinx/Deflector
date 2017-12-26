using GameEvents;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIPanel : MonoBehaviour, IOnUpdate {

    public UIPanelLink     panelLink;
    
    [Header("Events Out")]
    public GameEvent       onEnterPanel;
    public GameEvent       onClosePanel;
    
    protected UIController ui;
    protected UHooks       hooks;
    private Canvas         canvas;

    private void Awake() {
        canvas = GetComponent<Canvas>();
        panelLink.SetPanel(this);
    }

    public void Init(UIController uiController, UHooks uhooks) {
        ui = uiController;
        hooks = uhooks;
    }

    protected void Hide() {
        canvas.enabled = false;
    }

    protected void Show() {
        canvas.enabled = true;
    }
    
    public virtual void Open() {
        Show();
        hooks.AddOnUpdate(this);
        onEnterPanel?.Invoke();
    }

    public virtual void Close() {
        Hide();
        hooks.RemoveOnUpdate(this);
        onClosePanel?.Invoke();
    }

    public virtual void UOnUpdate() {}
}
