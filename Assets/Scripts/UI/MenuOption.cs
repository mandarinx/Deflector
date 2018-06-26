using GameEvents;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Deflector {

    public class MenuOption : MonoBehaviour {

        [SerializeField]
        private Image      selection;
        [SerializeField]
        private Text       label;
        [SerializeField]
        private ColorAsset defaultColor;
        [SerializeField]
        private ColorAsset selectedColor;
        [SerializeField]
        private GameEvent  onClick;

        private void Awake() {
            Assert.IsNotNull(selection, $"MenuOption {name} is missing a reference to the selection Image");
            Assert.IsNotNull(label, $"MenuOption {name} is missing a reference to the label");
            Assert.IsNotNull(defaultColor, $"MenuOption {name} is missing a reference to a default color");
            Assert.IsNotNull(selectedColor, $"MenuOption {name} is missing a reference to a selection color");
        }

        public void Select() {
            selection.enabled = true;
            label.color = selectedColor.Color;
        }

        public void Deselect() {
            selection.enabled = false;
            label.color = defaultColor.Color;
        }

        public void Click() {
            if (onClick == null) {
                return;
            }
            onClick.Invoke();
        }
    }
}
