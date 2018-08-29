using GameEvents;
using UnityEngine;

namespace Deflector {

    public class ToggleLanguage : MonoBehaviour {

        [SerializeField]
        private SystemLanguage[] languages;
        [SerializeField]
        private IntEvent onSetLanguage;

        private int curLanguage;

        public void Toggle() {
            ++curLanguage;
            curLanguage %= languages.Length;
            Debug.Log($"Toggle to: {languages[curLanguage]}");
            onSetLanguage.Invoke((int)languages[curLanguage]);
        }
    }
}
