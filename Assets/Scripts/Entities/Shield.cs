using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Shield : MonoBehaviour {
    
    [SerializeField]
    private Color                activeColor;
    [SerializeField]
    private float                hitRadius;
    [SerializeField]
    private Vector2              hitOffset;
    [SerializeField]
    private LayerMask            hitLayer;
    
    private SpriteRenderer       sr;
    private Collider2D[]         overlaps;
    
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        overlaps = new Collider2D[8];
    }

    private void Update() {
        sr.color = Overlap() > 0 ? activeColor : Color.white;
    }
    
    public void Hit(int angleIndex) {
        int numOverlaps = Overlap();
        for (int i = 0; i < numOverlaps; ++i) {
            overlaps[i].GetComponent<Hitable>()?.Hit(angleIndex);
        }
    }

    private int Overlap() {
        return Physics2D.OverlapCircleNonAlloc(transform.position + (Vector3)hitOffset, 
                                               hitRadius,
                                               overlaps,
                                               hitLayer.value);
    }

    private void OnDrawGizmosSelected() {
        GizmoUtils.DrawCircle(transform.position + (Vector3)hitOffset, hitRadius, Color.white);
    }
}
