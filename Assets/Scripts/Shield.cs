using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Shield : MonoBehaviour {

    public Color           activeColor;
    
    private Projectile     overlapped;
    private SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        overlapped = other.gameObject.GetComponent<Projectile>();
        sr.color = activeColor;
    }

    private void OnTriggerExit2D(Collider2D other) {
        overlapped = null;
        sr.color = Color.white;
    }

    public Projectile GetOverlapped() {
        return overlapped;
    }
}
