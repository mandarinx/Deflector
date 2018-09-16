using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Deflector {

    [RequireComponent(typeof(VideoPlayer))]
    public class VideoListener : MonoBehaviour {

        [SerializeField]
        private UnityEvent onVideoEnd;

        private void Awake() {
            GetComponent<VideoPlayer>().loopPointReached += OnEndReached;
        }

        private void OnEndReached(VideoPlayer source) {
            onVideoEnd.Invoke();
        }
    }
}
