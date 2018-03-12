using UnityEngine;
using UnityEngine.UI;

namespace Deflector {
    [RequireComponent(typeof(Text))]
    public class ScoreLabel : MonoBehaviour {

        [SerializeField]
        private IntAsset score;

        private Text     label;

        private void OnEnable() {
            label = GetComponent<Text>();
            score.AddChangeCallback(OnScoreChanged);
        }

        private void OnScoreChanged(int value) {
            label.text = value.ToString("### ### ##0");
        }
    }
}
