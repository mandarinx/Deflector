using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class MusicTrackk {
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
}

public class TestMusic : MonoBehaviour {

    public AudioMixer mixer;
    public MusicTrackk[] tracks;
    private AudioSource[] sources;
    private bool[] restart = { true, false, false };
    private string[] snapshots = { "InMenu", "InGame", "Idle" };
    private float[] muffle = { 5000f, 500f };
    private int curMuffle;

    private void Awake() {
        sources = new AudioSource[tracks.Length];
        for (int i = 0; i < tracks.Length; ++i) {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].clip = tracks[i].clip;
            sources[i].outputAudioMixerGroup = tracks[i].mixerGroup;
            sources[i].playOnAwake = false;
        }
    }

    private void Start() {
        Muffle(0.1f);
    }

    public void PlayTrack(int t, float d) {
        string snapshot = snapshots[t];
        mixer.FindSnapshot(snapshot).TransitionTo(d);
        if (restart[t]) {
            sources[t].Play();
            return;
        }

        if (sources[t].isPlaying) {
            return;
        }
        sources[t].Play();
    }

    public void Muffle(float duration) {
        curMuffle = ++curMuffle % muffle.Length;
        StartCoroutine(FadeMuffle(muffle[curMuffle], duration));
    }

    private IEnumerator FadeMuffle(float target, float duration) {
        float cur;
        mixer.GetFloat("Muffle", out cur);
        float dt = target - cur;
        float time = 0f;
        while (time < duration) {
            mixer.SetFloat("Muffle", cur + (time / duration) * dt);
            time += Time.deltaTime / duration;
            yield return null;
        }
        mixer.SetFloat("Muffle", cur + dt);
    }
}
