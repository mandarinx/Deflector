using UnityEngine;
using UnityEngine.UI;

namespace LunchGame01 {
    [RequireComponent(typeof(Image))]
    public class UIHeart : MonoBehaviour {

        [SerializeField]
        private Sprite alive;
        [SerializeField]
        private Sprite dead;

        private Image  image;

        private void OnEnable() {
            image = GetComponent<Image>();
            image.sprite = alive;
        }

        public bool isAlive {
            set { GetComponent<Image>().sprite = value ? alive : dead; }
            get { return GetComponent<Image>().sprite == alive; }
        }
    }
}
