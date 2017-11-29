using UnityEngine;
using UnityEngine.Events;

public class UIPanel : MonoBehaviour {

    public UnityEvent onEnter;
    public UnityEvent onClose;
    
    public void OnEnter() {
        onEnter.Invoke();
    }

    public void OnClose() {
        onClose.Invoke();
    }
}
