using System;
using UnityEngine;

namespace Deflector {
    [CreateAssetMenu(menuName = "Data/Float Asset", fileName = "FloatAsset.asset")]
    public class FloatAsset : ScriptableObject {

        [SerializeField]
        private float         curValue;
        [SerializeField]
        private float         initValue;
        [SerializeField]
        private FloatEvent    onValueChanged;

        private Action<float> onValueChangedCallback = i => { };

        public float Value => curValue;

        private void OnEnable() {
            curValue = initValue;
        }

        public void SetValue(float val) {
            bool changed = Math.Abs(val - curValue) > float.Epsilon;
            curValue = val;
            if (!changed) {
                return;
            }

            if (onValueChanged != null) {
                onValueChanged.Invoke(curValue);
            }
            onValueChangedCallback.Invoke(curValue);
        }

        public void AddChangeCallback(Action<float> callback) {
            onValueChangedCallback -= callback;
            onValueChangedCallback += callback;
        }

        public void RemoveChangeCallback(Action<float> callback) {
            onValueChangedCallback -= callback;
        }
    }
}
