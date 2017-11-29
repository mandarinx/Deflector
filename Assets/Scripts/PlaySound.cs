using System;
using UnityEngine;
using UnityEngine.Audio;
using Mandarin;
using Object = UnityEngine.Object;

namespace HyperGames.Lib {
    
    [Serializable]
    public struct AudioSourceConfig {
        public float            minDistance;
        public float            maxDistance;
        public float            spatialBlend;
        public float            dopplerLevel;
        public float            spread;
        public AudioRolloffMode rolloffMode;
    }
    
    [AddComponentMenu("Hyper Games/Lib/Play Sound")]
    public class PlaySound : MonoBehaviour {

        [SerializeField]
        private Camera            cam;
        [SerializeField]
        [Min(1)]
        private int               numAudioSources;
        [SerializeField]
        private AudioMixerGroup   output;
        [SerializeField]
        [Range(0f, 1f)]
        private float             volume;
        [SerializeField]
        private AudioSourceConfig audioSourceConfig;

        private AudioSource[]     audioSources;

        private void Start() {
            if (numAudioSources <= 0) {
                numAudioSources = 1;
            }

            audioSources = new AudioSource[numAudioSources];
            for (int i=0; i<numAudioSources; i++) {
                audioSources[i] = GO
                    .Create("AudioSource_"+i)
                    .SetParent(transform, false)
                    .AddComponent<AudioSource>(OnInitAudioSource)
                    .GetComponent<AudioSource>();
            }
        }

        private void OnInitAudioSource(AudioSource source) {
            source.playOnAwake = false;
			source.minDistance = audioSourceConfig.minDistance;
            source.maxDistance = audioSourceConfig.maxDistance;
            source.spatialBlend = audioSourceConfig.spatialBlend;
            source.dopplerLevel = audioSourceConfig.dopplerLevel;
            source.spread = audioSourceConfig.spread;
			source.rolloffMode = audioSourceConfig.rolloffMode;
        }

        public void Play(Object obj) {
            Play(obj as AudioClip);
        }

        public void Play(AudioClip audioClip) {
            AudioSource source = GetNextAudioSource();
            if (source == null) {
                return;
            }
            source.outputAudioMixerGroup = output;
            source.volume = volume;
            source.clip = audioClip;
            source.Play();
        }

        private AudioSource GetNextAudioSource() {
            for (int i=0; i<audioSources.Length; i++) {
                if (!audioSources[i].isPlaying) {
                    return audioSources[i];
                }
            }
            return null;
        }
    }
}

