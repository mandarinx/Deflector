using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Deflector {
    [RequireComponent(typeof(Image))]
    public class ImageColorLerp : MonoBehaviour {

        public Color      targetColor;
        public float      duration;
        public UnityEvent onLerpDone;

        private Image     image;
        private float     lerpTime;
        private Color     defaultColor;

        private void Awake() {
            image = GetComponent<Image>();
            defaultColor = image.color;
        }

        public void Lerp() {
            StartCoroutine(LerpColor());
        }

        public void RevertColor() {
            image.color = defaultColor;
        }

        public IEnumerator LerpColor() {
            lerpTime = Time.time;
            float stopTime = lerpTime + duration;

            while (lerpTime < stopTime) {
                float f = 1 - ((stopTime - lerpTime) / duration);
                image.color = Color.Lerp(defaultColor, targetColor, f);
                lerpTime += Time.deltaTime;
                yield return null;
            }

            onLerpDone.Invoke();
        }
    }
}
