using System.Collections;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

namespace LunchGame01 {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour {

        [SerializeField]
        private float                     speedNeutral = 1f;
        [SerializeField]
        private float                     speedCharged = 2f;
        [SerializeField]
        private LayerMask                 hitLayer;
        [SerializeField]
        private Gradient                  colorsNeutral;
        [SerializeField]
        private Gradient                  colorsCharged;
        [SerializeField]
        private GameObjectEvent           onExploded;
        [SerializeField]
        private GameObjectEvent           onHit;

        private Gradient                  colorsFlashing;
        private float                     angle;
        private float                     speed;
        private int                       contacts;
        private int                       angleIndex;

        // Merge these two into a state enum, or even a bitmask!
        private int                       activated;
        private bool                      exploding;

        private Vector2                   hitNormal;
        private Rigidbody2D               rb;
        private ParticleSystem            ps;
        private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[2];

        // Figure out an other way
        public bool isActivated => activated > 0;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            ps = GetComponent<ParticleSystem>();
            colorsFlashing = new Gradient {
                colorKeys = new[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                }
            };
        }

        private void OnEnable() {
            exploding = false;
            activated = 0;
            speed = speedNeutral;
            angleIndex = Random.Range(0, 4) * 2 + 1;
            angle = Angles.GetAngle(angleIndex);
            SetPSysColorLifetime(ps, colorsNeutral);
        }

        private void OnDisable() {
            StopAllCoroutines();
        }

        /// <summary>
        /// Called by Hitable.onHit
        /// </summary>
        /// <param name="hitAngleIndex">The angle index of the hit</param>
        [UsedImplicitly]
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
            SetPSysColorLifetime(ps, colorsCharged);
            speed = speedCharged;

            angleIndex %= 8;
            if (angleIndex < 0) {
                angleIndex = 8 + angleIndex;
            }
            angle = Angles.GetAngle(angleIndex);
        }

        /// <summary>
        /// Called by Killable.onKilled
        /// </summary>
        /// <param name="pos">The position of the killer</param>
        [UsedImplicitly]
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
                SetPSysColorLifetime(ps, colorsFlashing);
                yield return new WaitForSeconds(0.2f);
                SetPSysColorLifetime(ps, colorsCharged);
                yield return new WaitForSeconds(0.2f);
                ++blink;
            }

            onExploded.Invoke(gameObject);
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

        private static void SetPSysColorLifetime(ParticleSystem particleSys,
                                                 Gradient       gradient) {
            ParticleSystem.ColorOverLifetimeModule colOverLife = particleSys.colorOverLifetime;
            colOverLife.color = gradient;
        }
    }
}
