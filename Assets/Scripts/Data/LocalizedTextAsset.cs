using System;
using UnityEngine;

namespace Deflector {

    [CreateAssetMenu(menuName = "Data/Localized Text", fileName = "LocalizedTextAsset.asset")]
    public class LocalizedTextAsset : ScriptableObject {

        [SerializeField]
        private SystemLanguage[] languages;
        [SerializeField]
        private string[]         texts;

        public string GetLocalizedText(SystemLanguage lang) {
            int n = Array.IndexOf(languages, lang);
            return n < 0 ? $"[LANG {lang} NOT FOUND]" : texts[n];
        }
    }
}
