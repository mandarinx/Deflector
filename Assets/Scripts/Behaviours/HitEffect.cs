using UnityEngine;
using GameEvents;
using PowerTools;

namespace LunchGame01 {
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

        private void OnAnimDone() {
            onAnimDone.Invoke(gameObject);
        }
    }
}
