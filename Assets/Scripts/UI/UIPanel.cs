using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
public class UIPanel : MonoBehaviour, IOnUpdate {

    public UIPanelLink     panelLink;
    
    [Header("Events Out")]
    public UnityEvent      onEnterPanel;
    public UnityEvent      onClosePanel;
    
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

    public void Hide() {
        canvas.enabled = false;
    }

    public void Show() {
        canvas.enabled = true;
    }
    
    public virtual void Open() {
        Show();
        hooks.AddOnUpdate(this);
        onEnterPanel.Invoke();
    }

    public virtual void Close() {
        Hide();
        hooks.RemoveOnUpdate(this);
        onClosePanel.Invoke();
    }

    public virtual void UOnUpdate() {}
}
