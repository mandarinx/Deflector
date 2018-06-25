using UnityEngine;

namespace Deflector {

    [CreateAssetMenu(menuName = "Data/UI Panel Ref", fileName = "UIPanelRef.asset")]
    public class UIPanelRef : ScriptableObject {

        public UIPanel Panel { get; private set; }

        public void SetPanel(UIPanel uiPanel) {
            Panel = uiPanel;
        }
    }
}
