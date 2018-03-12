using UnityEngine;
using UnityEngine.UI;

namespace Deflector {
    [RequireComponent(typeof(Image))]
    public class UIPaddle : MonoBehaviour {

        [SerializeField]
        private Sprite full;
        [SerializeField]
        private Sprite empty;

        private Image  image;

        private void OnEnable() {
            image = GetComponent<Image>();
            image.sprite = full;
        }

        public bool isFull {
            set { GetComponent<Image>().sprite = value ? full : empty; }
            get { return GetComponent<Image>().sprite == full; }
        }
    }
}
