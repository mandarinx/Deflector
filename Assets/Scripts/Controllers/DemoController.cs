using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Deflector {
    public class DemoController : MonoBehaviour, IOnUpdate {

        [SerializeField]
        private AudioMixer      mixer;
        [SerializeField]
        private string          mixerFloatDemoVideo;
        [SerializeField]
        private string          mixerFloatMusic;
        [SerializeField]
        private UHooks          uHooks;
        [SerializeField]
        private UnityEvent      onDemoWillStart;
        [SerializeField]
        private UnityEvent      onDemoWillStop;

        public void StartDemo() {
            mixer.SetFloat(mixerFloatMusic,     -80f);
            mixer.SetFloat(mixerFloatDemoVideo,   0f);
            uHooks.AddOnUpdate(this);
            onDemoWillStart.Invoke();
        }

        public void StopDemo() {
            mixer.SetFloat(mixerFloatMusic,       0f);
            mixer.SetFloat(mixerFloatDemoVideo, -80f);
            onDemoWillStop.Invoke();
            uHooks.RemoveOnUpdate(this);
        }

        public void UOnUpdate() {
            if (!InputController.anyKey) {
                return;
            }
            StopDemo();
        }
    }
}
