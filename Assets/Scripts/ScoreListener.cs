using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreListener : MonoBehaviour {

    public IntAsset score;

    private Text label;
    
    private void Start() {
        label = GetComponent<Text>();
        score.OnValueChanged(OnScoreChanged);
    }

    private void OnScoreChanged(int value) {
        label.text = value.ToString("### ### ##0");
    }
}
