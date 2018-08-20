using UnityEngine;
using UnityEngine.Audio;

namespace Deflector {

    [CreateAssetMenu(menuName = "Data/Music Track", fileName = "MusicTrack.asset")]
    public class MusicTrack : ScriptableObject {

        [SerializeField]
        private AudioClip       audioClip;
        [SerializeField]
        private AudioMixerGroup mixerGroup;
        [SerializeField]
        private string          snapshot;
        [SerializeField]
        private bool            loop;

        public AudioClip        AudioClip       => audioClip;
        public AudioMixerGroup  MixerGroup      => mixerGroup;
        public string           Snapshot        => snapshot;
        public bool             Loop            => loop;
    }
}
