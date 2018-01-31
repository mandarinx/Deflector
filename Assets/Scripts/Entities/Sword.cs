using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sword : MonoBehaviour {

    [SerializeField]
    private Color            activeColor;

    private SpriteRenderer   sr;
    private List<Collider2D> overlapped;

    private void Awake() {
        overlapped = new List<Collider2D>(16);
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!overlapped.Contains(other)) {
            overlapped.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        overlapped.Remove(other);
    }

    private void Update() {
        sr.color = overlapped.Count > 0 ? activeColor : Color.white;
    }

    public void Hit(int angleIndex) {
        for (int i = 0; i < overlapped.Count; ++i) {
            overlapped[i].GetComponent<Hitable>()?.Hit(angleIndex);
        }
    }
}
