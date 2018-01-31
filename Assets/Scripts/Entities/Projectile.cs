using System.Collections;
using GameEvents;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {

    [Tooltip("Angle in radians")]
    [SerializeField]
    private float                     angle;
    [SerializeField]
    private float                     speed = 1f;
    [SerializeField]
    private LayerMask                 hitLayer;
    [SerializeField]
    private Sprite[]                  sprites;
    [SerializeField]
    private GameObjectEvent           onDespawn;
    [SerializeField]
    private GameObjectEvent           onHit;

    private int                       contacts;
    private int                       angleIndex;
    private int                       activated;
    private bool                      exploding;
    private Vector2                   hitNormal;
    private Rigidbody2D               rb;
    private SpriteRenderer            sr;
    private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[2];

    public bool isActivated => activated > 0;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        exploding = false;
        activated = 0;
        speed = 1f;
        angleIndex = Random.Range(0, 4) * 2 + 1;
        angle = Angles.GetAngle(angleIndex);
        sr.sprite = sprites[angleIndex + activated];
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    // Called by Hitable
    public void Hit(int hitAngleIndex) {
        if (exploding) {
            return;
        }

        onHit.Invoke(gameObject);

        if (activated == 8) {
            StartCoroutine(Explosion(4));
        }

        int angleDiff = 8 - angleIndex;
        int hitAngleRelative = (hitAngleIndex + angleDiff) % 8;

//        if (hitAngleRelative == 0) {
//            Debug.Log("from behind");
//        }

        if (hitAngleRelative == 4) {
            angleIndex += 4;
        }

        if (hitAngleRelative > 0 && hitAngleRelative < 4) {
            angleIndex += 2;
        }

        if (hitAngleRelative > 4 && hitAngleRelative < 8) {
            angleIndex -= 2;
        }

        activated = 8;
        speed += 1f;

        angleIndex %= 8;
        if (angleIndex < 0) {
            angleIndex = 8 + angleIndex;
        }
        angle = Angles.GetAngle(angleIndex);
        sr.sprite = sprites[angleIndex + activated];
    }

    // Called by Killable
    public void Explode(Vector3 pos) {
        if (exploding) {
            return;
        }

        StartCoroutine(Explosion(2));
    }

    private IEnumerator Explosion(int blinks) {
        exploding = true;
        int blink = 0;
        while (blink < blinks) {
            sr.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.2f);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.2f);
            ++blink;
        }

        onDespawn.Invoke(gameObject);
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + GetVelocity() * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!LayerMasks.LayerInMask(collision.gameObject.layer, hitLayer)) {
            return;
        }

        if (!GetHitNormal(collision, out hitNormal)) {
            return;
        }

        angleIndex = GetAngleIndex(angleIndex, hitNormal, GetVelocity().normalized);
        angle = Angles.GetAngle(angleIndex);
        sr.sprite = sprites[angleIndex + activated];
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!LayerMasks.LayerInMask(collision.gameObject.layer, hitLayer)) {
            return;
        }

        // compare hit normals, if not the same, calc new angleindex
        Vector2 normal;
        if (!GetHitNormal(collision, out normal)) {
            return;
        }
        if (normal == hitNormal) {
            return;
        }
        int index = GetAngleIndex(angleIndex, hitNormal, GetVelocity().normalized);
        if (index == angleIndex) {
            return;
        }
        angleIndex = index;
        angle = Angles.GetAngle(angleIndex);
        sr.sprite = sprites[angleIndex + activated];
    }

    private bool GetHitNormal(Collision2D collision, out Vector2 normal) {
        contacts = collision.GetContacts(contactPoints);
        if (contacts == 0) {
            normal = Vector2.zero;
            return false;
        }

        normal = Vector2.zero;
        for (int i = 0; i < contacts; ++i) {
            normal += contactPoints[i].normal.normalized;
        }
        return true;
    }

    // TODO: Should be called something like DeflectAngleIndex
    private static int GetAngleIndex(int angleIndex, Vector2 hitNormal, Vector2 velocity) {
        float dot = Vector2.Dot(hitNormal, velocity);
        //  1 = same direction
        // -1 = opposite direction
        //  0 = perpendicular

        // Bounce back
        if (dot < -0.995f) {
            angleIndex += 4;
        }

        // Bounce to either side
        if ((dot < +0.7853f && dot > +float.Epsilon) ||
            (dot > -0.7853f && dot < -float.Epsilon)) {
            Vector3 cross = Vector3.Cross(hitNormal, velocity).normalized;
            // To right
            if (cross == Vector3.back) {
                angleIndex += 2;
            }
            // To left
            if (cross == Vector3.forward) {
                angleIndex -= 2;
            }
        }

        // Follow the normal
        if (dot >= -float.Epsilon &&
            dot <= +float.Epsilon) {
            // convert normal to angle index
            angleIndex = Angles.GetAngleIndex(Angles.GetRadian(hitNormal));
        }

        // Follow the velocity
        if (dot >= +0.7853f && dot <= 1f) {
            // do nothing
        }

        angleIndex %= 8;
        if (angleIndex < 0) {
            angleIndex = 8 + angleIndex;
        }

        return angleIndex;
    }

    private Vector2 GetVelocity() {
        return Angles.GetDirection(angle) * speed;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, GetVelocity());

        Gizmos.color = Color.cyan;
        for (int i=0; i<contacts; ++i) {
            Gizmos.DrawRay(contactPoints[i].point, contactPoints[i].normal);
        }
    }
}
