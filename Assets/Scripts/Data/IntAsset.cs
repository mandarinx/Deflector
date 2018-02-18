using System;
using UnityEngine;

namespace LunchGame01 {
    [CreateAssetMenu(menuName = "Data/Int Asset", fileName = "IntAsset.asset")]
    public class IntAsset : ScriptableObject {

        [SerializeField]
        private int         curValue;
        [SerializeField]
        private int         initValue;

        private Action<int> onValueChanged = i => { };

        public int value => curValue;

        private void OnEnable() {
            curValue = initValue;
        }

        public void SetValue(int val) {
            bool changed = val != curValue;
            curValue = val;
            if (changed) {
                onValueChanged.Invoke(curValue);
            }
        }

        public void OnValueChanged(Action<int> callback) {
            onValueChanged += callback;
        }
    }
}
