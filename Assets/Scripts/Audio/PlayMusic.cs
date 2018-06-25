using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {

    public class PlayMusic : MonoBehaviour {

        [SerializeField]
        private MusicTrack  track;
        [SerializeField]
        private float       fadeDuration;
        [SerializeField]
        private MusicPlayer musicPlayer;

        [UsedImplicitly]
        public void Play() {
            musicPlayer.PlayTrack(track, fadeDuration);
        }
    }
}
