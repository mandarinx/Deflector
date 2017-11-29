using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MultiplierListener : MonoBehaviour {

    public IntAsset multiplier;

    private Text label;
    
    private void Start() {
        label = GetComponent<Text>();
        multiplier.OnValueChanged(OnMultiplierChanged);
    }

    private void OnMultiplierChanged(int value) {
        label.text = value.ToString("###") + "x";
    }
}
