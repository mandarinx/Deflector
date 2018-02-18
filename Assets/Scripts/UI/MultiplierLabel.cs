using UnityEngine;
using UnityEngine.UI;

namespace LunchGame01 {
    [RequireComponent(typeof(Text))]
    public class MultiplierLabel : MonoBehaviour {

        [SerializeField]
        private IntAsset multiplier;

        private Text    label;

        private void Start() {
            label = GetComponent<Text>();
            multiplier.OnValueChanged(OnMultiplierChanged);
        }

        private void OnMultiplierChanged(int value) {
            label.text = value > 1 ? $"{value:###}x" : "";
        }
    }
}
