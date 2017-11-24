using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour {
    
    [Tooltip("Angle in radians")]
    [SerializeField]
    private float angle;
    [SerializeField]
    private float speed = 1f;

    private Rigidbody2D rb;
    private ContactPoint2D[] contactPoints = new ContactPoint2D[1];
    private const float halfPI = Mathf.PI / 2;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        angle = Random.Range(1, 5) * (Mathf.PI / 2) + (Mathf.PI / 4);
    }

    private void FixedUpdate() {
        Vector2 velocity = new Vector2(
            Mathf.Sin(angle) * speed,
            Mathf.Cos(angle) * speed
        );
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        int contacts = other.GetContacts(contactPoints);
        if (contacts == 0) {
            return;
        }
        
        Vector2 velocity = new Vector2(
            Mathf.Sin(angle) * speed,
            Mathf.Cos(angle) * speed
        );

        if (velocity.x > 0) {
            if (contactPoints[0].normal == Vector2.up) {
                angle -= halfPI;
            }
            if (contactPoints[0].normal == Vector2.down) {
                angle += halfPI;
            }
            if (contactPoints[0].normal == Vector2.left) {
                angle += velocity.y > 0 ? -halfPI : halfPI;
            }
        } else {
            if (contactPoints[0].normal == Vector2.up) {
                angle += halfPI;
            }
            if (contactPoints[0].normal == Vector2.down) {
                angle -= halfPI;
            }
            if (contactPoints[0].normal == Vector2.right) {
                angle += velocity.y > 0 ? halfPI : -halfPI;
            }
        }

        angle %= Mathf.PI * 2;
    }

    private void OnDrawGizmos() {
        float x = Mathf.Sin(angle) * 2;
        float y = Mathf.Cos(angle) * 2;
        Gizmos.DrawRay(transform.position, new Vector2(x, y));
    }
}
