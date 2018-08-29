using System;
using GameEvents;
using UnityEngine;

namespace Deflector {

    public class LocalizationController : MonoBehaviour {

        private const string PREFS_KEY = "DEFLECTOR_LANG";

        [SerializeField]
        private SystemLanguage[]    validLanguages;
        [SerializeField]
        private SystemLanguage      defaultLanguage;
        [SerializeField]
        private IntEvent            onLanguageChanged;

        private void Start() {
            int curLang = PlayerPrefs.HasKey(PREFS_KEY)
                ? PlayerPrefs.GetInt(PREFS_KEY)
                : (int) Application.systemLanguage;

            curLang = SanitizeLanguage(curLang);
            PlayerPrefs.SetInt(PREFS_KEY, curLang);
            onLanguageChanged.Invoke(curLang);
        }

        private int SanitizeLanguage(int lang) {
            return Array.IndexOf(validLanguages, (SystemLanguage) lang) < 0
                ? (int) defaultLanguage
                : lang;
        }

        public void SetLanguage(int langId) {
            langId = SanitizeLanguage(langId);
            if (PlayerPrefs.GetInt(PREFS_KEY) == langId) {
                return;
            }
            PlayerPrefs.SetInt(PREFS_KEY, langId);
            onLanguageChanged.Invoke(langId);
        }
    }
}
