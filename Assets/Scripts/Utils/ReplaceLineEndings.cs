using UnityEngine;
using UnityEngine.UI;

namespace Deflector {
    public class ReplaceLineEndings : MonoBehaviour {

        [SerializeField]
        private string encodedEnding;
        [SerializeField]
        private Text   label;

        public void Replace(string str) {
            label.text = str.Replace(encodedEnding, "\n");
        }
    }
}
