using System.Collections.Generic;
using UnityEngine;

namespace LunchGame01 {
    [RequireComponent(typeof(SpriteRenderer))]
    public class HitPoint : MonoBehaviour {

        [SerializeField]
        private Color            activeColor;

        private SpriteRenderer   sr;
        private List<Collider2D> overlapped;

        private void Awake() {
            overlapped = new List<Collider2D>(16);
            sr = GetComponent<SpriteRenderer>();
        }

        public void Hit(int angleIndex) {
            for (int i = 0; i < overlapped.Count; ++i) {
                overlapped[i].GetComponent<Hitable>()?.Hit(angleIndex);
            }
        }

        public void Hide() {
            sr.enabled = false;
        }

        public void Show() {
            sr.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!overlapped.Contains(other)) {
                overlapped.Add(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            overlapped.Remove(other);
        }

        private void Update() {
            sr.color = overlapped.Count > 0 ? activeColor : Color.white;
        }
    }
}
