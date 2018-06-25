using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {

    public class FadeLowPassCutOff : MonoBehaviour {

        [SerializeField]
        private float       duration;
        [SerializeField]
        private MusicPlayer musicPlayer;

        [UsedImplicitly]
        public void Fade() {
            musicPlayer.FadeLowPassCutOff(duration);
        }
    }
}
