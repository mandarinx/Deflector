using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Deflector {

    public class UIOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler {

        [SerializeField]
        private Graphic[]  graphics;
        [SerializeField]
        private Color      selected;
        [SerializeField]
        private Color      deselected;

        private Selectable selectable;

        private void Awake() {
            selectable = GetComponent<Selectable>();
            Assert.IsNotNull(selectable, "Could not find a Selectable");
        }

        public void OnSelect(BaseEventData eventData) {
            for (int i = 0; i < graphics.Length; ++i) {
                graphics[i].color = selected;
            }
        }

        public void OnDeselect(BaseEventData eventData) {
            for (int i = 0; i < graphics.Length; ++i) {
                graphics[i].color = deselected;
            }
        }
    }
}
