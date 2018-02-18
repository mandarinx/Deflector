using UnityEngine;
using UnityEngine.Events;

namespace LunchGame01 {
    [RequireComponent(typeof(Canvas))]
    public class UIPanel : MonoBehaviour {

        [SerializeField]
        private UIPanelLink     panelLink;
        [SerializeField]
        private UnityEvent      onEnterPanel;
        [SerializeField]
        private UnityEvent      onClosePanel;

        private Canvas          canvas;

        private void Awake() {
            canvas = GetComponent<Canvas>();
            panelLink.SetPanel(this);
        }

        public void Hide() {
            canvas.enabled = false;
        }

        public void Show() {
            canvas.enabled = true;
        }

        public virtual void Open() {
            Show();
            onEnterPanel.Invoke();
        }

        public virtual void Close() {
            Hide();
            onClosePanel.Invoke();
        }
    }
}
