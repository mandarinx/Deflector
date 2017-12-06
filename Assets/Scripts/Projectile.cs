using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    
    [Tooltip("Angle in radians")]
    [SerializeField]
    private float                     angle;
    [SerializeField]
    private float                     speed = 1f;
    [SerializeField]
    private Sprite[]                  sprites;
    [SerializeField]
    private GameObject                explosionPrefab;
    [SerializeField]
    private GameObjectEvent           onDespawn;
    [SerializeField]
    private GameEvent                 onHit;

    private int                       angleIndex;
    [SerializeField]
    private int                       activated;
    private bool                      exploding;
    private Rigidbody2D               rb;
    private SpriteRenderer            sr;
    private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[2];

    private readonly Dictionary<int, float> radianMap = new Dictionary<int, float> {
        { 0, 0f },
        { 1, Mathf.PI * 0.25f },
        { 2, Mathf.PI * 0.5f },
        { 3, Mathf.PI * 0.75f },
        { 4, Mathf.PI },
        { 5, Mathf.PI * 1.25f },
        { 6, Mathf.PI * 1.5f },
        { 7, Mathf.PI * 1.75f },
    };

    public bool isActivated => activated > 0;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        exploding = false;
        activated = 0;
        angleIndex = Random.Range(0, 4) * 2 + 1;
        angle = radianMap[angleIndex];
        sr.sprite = sprites[angleIndex + activated];
    }

    public void Hit(int hitAngleIndex) {
        if (exploding) {
            return;
        }
        
        onHit.Raise();
        
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
        angle = radianMap[angleIndex];
        sr.sprite = sprites[angleIndex + activated];
    }

    public void Explode() {
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

        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<Explosion>().Explode();
        
        onDespawn.Raise(gameObject);
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + GetVelocity() * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        int contacts = collision.GetContacts(contactPoints);
        if (contacts == 0) {
            return;
        }

        Vector2 velocity = GetVelocity().normalized;
        Vector2 hitNormal = Vector2.zero;
        for (int i = 0; i < contacts; ++i) {
            hitNormal += contactPoints[i].normal.normalized;
        }
        
        float dot = Vector2.Dot(hitNormal, velocity);
        //  1 = same direction
        // -1 = opposite direction
        //  0 = perpendicular
        
        // Bounce back
        if (dot < -0.995f) {
            angleIndex += 4;
        }
        
        // Bounce to either side
        if (dot <= 0.995f && dot > 0f) {
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
        if (dot >= -0.995f && dot < 0f) {
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
        
        angleIndex %= 8;
        if (angleIndex < 0) {
            angleIndex = 8 + angleIndex;
        }
        angle = radianMap[angleIndex];
        sr.sprite = sprites[angleIndex + activated];
        rb.MovePosition(rb.position + (hitNormal.normalized * 0.04167f) * Time.fixedDeltaTime);
    }

    private Vector2 GetVelocity() {
        return new Vector2(
            Mathf.Cos(angle) * speed,
            Mathf.Sin(angle) * speed
        );
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, GetVelocity());
        
        Gizmos.color = Color.cyan;
        for (int i=0; i<contactPoints.Length; ++i) {
            Gizmos.DrawRay(contactPoints[i].point, contactPoints[i].normal);
        }
    }
}
