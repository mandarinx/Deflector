using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    
    [Tooltip("Angle in radians")]
    [SerializeField]
    private float      angle;
    [SerializeField]
    private float      speed = 1f;
    [SerializeField]
    private Sprite[]   sprites;
    [SerializeField]
    private GameObject explosion;

    private int angleIndex;
    private int activated;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[1];
    private const float halfPI = Mathf.PI / 2;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        activated = 0;
        int r = Random.Range(0, 4);
        angleIndex = r;
        angle = r * (Mathf.PI / 2) + (Mathf.PI / 4);
        sr.sprite = sprites[angleIndex + activated];
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + GetVelocity() * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // check for hit with player weapon
        
        int contacts = other.GetContacts(contactPoints);
        if (contacts == 0) {
            return;
        }

        Vector2 velocity = GetVelocity();

        if (velocity.x > 0) {
            if (contactPoints[0].normal == Vector2.up) {
                angle -= halfPI;
                --angleIndex;
            }
            if (contactPoints[0].normal == Vector2.down) {
                angle += halfPI;
                ++angleIndex;
            }
            if (contactPoints[0].normal == Vector2.left) {
                if (velocity.y > 0) {
                    angle -= halfPI;
                    --angleIndex;
                }
                else {
                    angle += halfPI;
                    ++angleIndex;
                }
            }
        } else {
            if (contactPoints[0].normal == Vector2.up) {
                angle += halfPI;
                ++angleIndex;
            }
            if (contactPoints[0].normal == Vector2.down) {
                angle -= halfPI;
                --angleIndex;
            }
            if (contactPoints[0].normal == Vector2.right) {
                if (velocity.y > 0) {
                    angle += halfPI;
                    ++angleIndex;
                }
                else {
                    angle -= halfPI;
                    --angleIndex;
                }
            }
        }

        angle %= Mathf.PI * 2;
        angleIndex %= 4;
        if (angleIndex < 0) {
            angleIndex = 4 + angleIndex;
        }
        Debug.Log($"angleIndex: {angleIndex} angle: {angleIndex * (Mathf.PI / 2) + Mathf.PI / 4}");
        sr.sprite = sprites[angleIndex + activated];
    }

    private Vector2 GetVelocity() {
        return new Vector2(
            Mathf.Cos(angle) * speed,
            Mathf.Sin(angle) * speed
        );
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, GetVelocity());
    }
}
