using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTrack : MonoBehaviour {

    public int track;
    public float duration;
    public TestMusic tm;

    public void OnClick() {
        tm.PlayTrack(track, duration);
    }
}
