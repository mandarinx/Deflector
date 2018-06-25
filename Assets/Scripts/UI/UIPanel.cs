using UnityEngine;
using UnityEngine.Events;

namespace Deflector {
    [RequireComponent(typeof(Canvas))]
    public class UIPanel : MonoBehaviour {

        [SerializeField]
        private UIPanelRef      panelRef;
        [SerializeField]
        private UnityEvent      onEnterPanel;
        [SerializeField]
        private UnityEvent      onClosePanel;
        [SerializeField]
        private MenuOption[]    menuOptions;
        [SerializeField]
        private UIPanelRef[]    panelRefs;

        private Canvas          canvas;

        private void Awake() {
            canvas = GetComponent<Canvas>();
            panelRef.SetPanel(this);
        }

        public void Open() {
            canvas.enabled = true;
            onEnterPanel.Invoke();
        }

        public void Close() {
            canvas.enabled = false;
            onClosePanel.Invoke();
        }

        public MenuOption[] MenuOptions => menuOptions;
        public UIPanelRef[] PanelRefs => panelRefs;
        public UIPanelRef   PanelRef => panelRef;
    }
}
