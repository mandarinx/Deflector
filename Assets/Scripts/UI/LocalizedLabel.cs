using System;
using GameEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Deflector {
    public class LocalizedLabel : MonoBehaviour {

        [Serializable]
        private class UnityStringEvent : UnityEvent<string> {}

        [SerializeField]
        private IntEvent           onLanguageChanged;
        [SerializeField]
        private LocalizedTextAsset localizedText;
        [SerializeField]
        private Text               label;
        [SerializeField]
        private UnityStringEvent   onLocalizedText;

        private void Awake() {
            onLanguageChanged.AddCallback(OnLanguageChanged);
        }

        private void OnLanguageChanged(int langId) {
            string locText = localizedText.GetLocalizedText((SystemLanguage)langId);
            if (label != null) {
                label.text = locText;
            }
            onLocalizedText.Invoke(locText);
        }
    }
}
