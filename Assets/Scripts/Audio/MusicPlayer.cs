using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Deflector {

    public class MusicPlayer : MonoBehaviour {

        [SerializeField]
        private AudioMixer    mixer;
        [SerializeField]
        private MusicTrack[]  tracks;
        [SerializeField]
        private float[]       lowPassCutOff;
        [SerializeField]
        private string        lowPassName;

        private AudioSource[] sources;
        private int           curCutOff;

        private void Awake() {
            sources = new AudioSource[tracks.Length];
            for (int i = 0; i < tracks.Length; ++i) {
                sources[i] = gameObject.AddComponent<AudioSource>();
                sources[i].clip = tracks[i].AudioClip;
                sources[i].outputAudioMixerGroup = tracks[i].MixerGroup;
                sources[i].playOnAwake = false;
            }
        }

        private void Start() {
            FadeLowPassCutOff(0f);
        }

        public void PlayTrack(MusicTrack track, float duration) {
            int t = Array.IndexOf(tracks, track);
            if (t < 0) {
                return;
            }

            mixer.FindSnapshot(track.Snapshot).TransitionTo(duration);

            if (track.RestartOnFadeIn) {
                sources[t].Play();
                return;
            }

            if (sources[t].isPlaying) {
                return;
            }

            sources[t].Play();
        }

        public void FadeLowPassCutOff(float duration) {
            StartCoroutine(FadeFloat(lowPassCutOff[curCutOff], duration));
            curCutOff = ++curCutOff % lowPassCutOff.Length;
        }

        private IEnumerator FadeFloat(float target, float duration) {
            float cur;
            mixer.GetFloat(lowPassName, out cur);
            float dt = target - cur;
            float time = 0f;
            while (time <= duration) {
                mixer.SetFloat(lowPassName, cur + (time / duration) * dt);
                time += Time.deltaTime / duration;
                yield return null;
            }
            mixer.SetFloat(lowPassName, cur + dt);
        }
    }
}
