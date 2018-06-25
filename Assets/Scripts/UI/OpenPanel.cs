using UnityEngine;

namespace Deflector {

    public class OpenPanel : MonoBehaviour {

        [SerializeField]
        private UIPanelRef   panelRef;
        [SerializeField]
        private UIController uiController;

        public void Open() {
            uiController.OpenPanel(panelRef);
        }
    }
}
