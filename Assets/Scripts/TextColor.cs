using UnityEngine;
using UnityEngine.UI;

namespace Deflector {
    [RequireComponent(typeof(Text))]
    public class TextColor : MonoBehaviour {

        [SerializeField]
        private Color targetColor;

        private Text  text;
        private Color originalColor;

        private void Awake() {
            text = GetComponent<Text>();
            originalColor = text.color;
        }

        public void OverrideColor() {
            text.color = targetColor;
        }

        public void ResetColor() {
            text.color = originalColor;
        }
    }
}
