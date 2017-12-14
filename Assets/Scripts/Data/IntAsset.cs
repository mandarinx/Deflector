using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Int Asset", fileName = "IntAsset.asset")]
public class IntAsset : ScriptableObject {
    
    [SerializeField]
    private int curValue;

    private Action<int> onValueChanged = i => { };

    public int value => curValue;

    public void SetValue(int value) {
        bool changed = value != curValue;
        curValue = value;
        if (changed) {
            onValueChanged.Invoke(curValue);
        }
    }
    
    public void OnValueChanged(Action<int> callback) {
        onValueChanged += callback;
    }
}
