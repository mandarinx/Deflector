using UnityEngine;
using UnityEngine.UI;

namespace Deflector {
    [RequireComponent(typeof(Text))]
    public class MultiplierLabel : MonoBehaviour {

        [SerializeField]
        private IntAsset multiplier;

        private Text    label;

        private void Start() {
            label = GetComponent<Text>();
            multiplier.AddChangeCallback(OnMultiplierChanged);
        }

        private void OnMultiplierChanged(int value) {
            label.text = value > 1 ? $"{value:###}x" : "";
        }
    }
}
