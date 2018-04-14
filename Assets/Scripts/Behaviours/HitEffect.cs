using UnityEngine;
using GameEvents;

namespace Deflector {
    public class HitEffect : MonoBehaviour {

        [SerializeField]
        private GameObjectEvent  onAnimDone;
        private SpriteAnimPlayer anim;

        private void Awake() {
            anim = GetComponent<SpriteAnimPlayer>();
            anim.OnDone(OnAnimDone);
        }

        public void Activate() {
            anim.Play();
        }

        private void OnAnimDone(AnimationClip clip) {
            onAnimDone.Invoke(gameObject);
        }
    }
}
