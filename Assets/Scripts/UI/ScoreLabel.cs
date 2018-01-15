using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreLabel : MonoBehaviour {

    public IntAsset score;

    private Text label;
    
    private void OnEnable() {
        label = GetComponent<Text>();
        score.OnValueChanged(OnScoreChanged);
    }

    private void OnScoreChanged(int value) {
        Debug.Log($"Score changed to {value}, label: {label}");
        label.text = value.ToString("### ### ##0");
    }
}
