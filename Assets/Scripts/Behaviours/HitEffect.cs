using UnityEngine;
using GameEvents;
using PowerTools;

namespace Deflector {
    public class HitEffect : MonoBehaviour {

        [SerializeField]
        private GameObjectEvent onAnimDone;
        private SpriteAnim      anim;

        private void Awake() {
            anim = GetComponent<SpriteAnim>();
            anim.AddDoneListener(OnAnimDone);
        }

        private void OnEnable() {
            anim.SetTime(0f);
            anim.Play(anim.Clip);
        }

        private void OnAnimDone(AnimationClip clip) {
            onAnimDone.Invoke(gameObject);
        }
    }
}
