using System;
using GameEvents;
using UnityEngine;

namespace Deflector {
    [CreateAssetMenu(menuName = "Data/Int Asset", fileName = "IntAsset.asset")]
    public class IntAsset : ScriptableObject {

        [SerializeField]
        private int         curValue;
        [SerializeField]
        private int         initValue;
        [SerializeField]
        private IntEvent    onValueChanged;

        private Action<int> onValueChangedCallback = i => { };

        public int value => curValue;

        private void OnEnable() {
            curValue = initValue;
        }

        public void SetValue(int val) {
            bool changed = val != curValue;
            curValue = val;
            if (!changed) {
                return;
            }
            onValueChanged?.Invoke(curValue);
            onValueChangedCallback.Invoke(curValue);
        }

        public void AddChangeCallback(Action<int> callback) {
            onValueChangedCallback -= callback;
            onValueChangedCallback += callback;
        }

        public void RemoveChangeCallback(Action<int> callback) {
            onValueChangedCallback -= callback;
        }
    }
}
