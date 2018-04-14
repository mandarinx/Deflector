using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Deflector {

    [RequireComponent(typeof(Animator))]
    public class SpriteAnimPlayer : MonoBehaviour {

        [Serializable]
        private class UnityAnimationClipEvent : UnityEvent<AnimationClip> {}

        [SerializeField]
        private AnimationClip              anim;
        [SerializeField]
        private SpriteAnimSharedController sharedController;
        [SerializeField]
        private bool                       playOnAwake;
        [SerializeField]
        private UnityAnimationClipEvent    onAnimDone;

        private AnimatorOverrideController overrideController;
        private Animator                   animator;
        private bool                       isDone;
        private List<KeyValuePair<AnimationClip, AnimationClip>> clipPairList;

        public bool IsPlaying => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;

        private void Awake() {
            animator = GetComponent<Animator>();

            overrideController = new AnimatorOverrideController {
                runtimeAnimatorController = sharedController.Controller
            };
            animator.runtimeAnimatorController = overrideController;

            clipPairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            overrideController.GetOverrides(clipPairList);

            if (playOnAwake) {
                Play();
            }
        }

        [ContextMenu("Play")]
        public void Play() {
            animator.enabled = true;
            isDone = false;
            clipPairList[0] = new KeyValuePair<AnimationClip, AnimationClip>(clipPairList[0].Key, anim);
            overrideController.ApplyOverrides(clipPairList);
            animator.Update(0.0f);
            animator.Play(sharedController.StateName, 0, 0f);
            animator.speed = 1f;
            animator.Update(0.0f);
        }

        public void Play(AnimationClip clip) {
            anim = clip;
            overrideController.GetOverrides(clipPairList);
            Play();
        }

        public void Stop() {
            animator.enabled = false;
        }

        public void OnDone(UnityAction<AnimationClip> cb) {
            onAnimDone.RemoveListener(cb);
            onAnimDone.AddListener(cb);
        }

        private void Update() {
            if (anim == null) {
                return;
            }

            if (IsPlaying) {
                return;
            }

            if (isDone) {
                return;
            }

            isDone = true;
            onAnimDone.Invoke(anim);
        }
    }
}
