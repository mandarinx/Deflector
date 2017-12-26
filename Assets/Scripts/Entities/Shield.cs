using GameEvents;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Shield : MonoBehaviour {

    public Color                 activeColor;
    public Vector3Event          shieldHitAt;
    
    private Hitable              hitable;
    private SpriteRenderer       sr;
    private new CircleCollider2D collider;
    
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        hitable = other.gameObject.GetComponent<Hitable>();
        sr.color = activeColor;
    }

    private void OnTriggerExit2D(Collider2D other) {
        hitable = null;
        sr.color = Color.white;
    }

    public void Hit(int angleIndex) {
        if (hitable == null) {
            return;
        }
        
        Vector3 delta = hitable.transform.position - transform.position;
        delta = delta.normalized * collider.radius;
        shieldHitAt.Invoke(transform.position + delta);
        
        hitable.Hit(angleIndex);
    }
}
