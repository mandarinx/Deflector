using System;
using System.Collections;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Deflector {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour {

        [Flags]
        private enum State {
            NEUTRAL   = 1,
            CHARGED   = 2,
            EXPLODING = 64,
        }

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
        private int                       angleIndex;
        private State                     state;
        private Rigidbody2D               rb;
        private ParticleSystem            ps;
        private CircleCollider2D          coll;
        private Vector2                   hitNormal;
        private readonly RaycastHit2D[]   hits = new RaycastHit2D[1];
        private float                     rayLen;
        private readonly Vector2[]        raycastDirs = {
            Vector2.up,
            (Vector2.up + Vector2.right).normalized,
            Vector2.right,
            (Vector2.down + Vector2.right).normalized,
            Vector2.down,
            (Vector2.down + Vector2.left).normalized,
            Vector2.left,
            (Vector2.up + Vector2.left).normalized,
        };

        public bool                       IsExploding => (state & State.EXPLODING) > 0;
        public bool                       IsCharged   => (state & State.CHARGED)   > 0;
        public bool                       IsNeutral   => (state & State.NEUTRAL)   > 0;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            ps = GetComponent<ParticleSystem>();
            coll = GetComponent<CircleCollider2D>();
            rayLen = coll.radius * 1.25f;
            colorsFlashing = new Gradient {
                colorKeys = new[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                }
            };
        }

        private void OnEnable() {
            state = State.NEUTRAL;
            speed = speedNeutral;
            angleIndex = Random.Range(0, 4) * 2 + 1;
            angle = Angles.GetAngle(angleIndex);
            SetPSysColorLifetime(ps, colorsNeutral);
            coll.enabled = true;
        }

        private void OnDisable() {
            coll.enabled = false;
            StopAllCoroutines();
        }

        /// <summary>
        /// Called by Hitable.onHit
        /// </summary>
        /// <param name="hitAngleIndex">The angle index of the hit</param>
        [UsedImplicitly]
        public void Hit(int hitAngleIndex) {
            if (IsExploding) {
                return;
            }

            onHit.Invoke(gameObject);

            if (IsCharged) {
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

            state = State.CHARGED;
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
            if (IsExploding) {
                return;
            }

            StartCoroutine(Explosion(2));
        }

        private IEnumerator Explosion(int blinks) {
            state |= State.EXPLODING;
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

            hitNormal = Vector2.zero;
            for (int i = 0; i < raycastDirs.Length; ++i) {
                hitNormal += Raycast(raycastDirs[i]);
            }

            if (hitNormal == Vector2.zero) {
                return;
            }

            angleIndex = DeflectAngleIndex(angleIndex, hitNormal, GetVelocity().normalized);
            angle = Angles.GetAngle(angleIndex);
            rb.MovePosition(rb.position + GetVelocity() * Time.fixedDeltaTime * 2f);
        }

        private Vector2 Raycast(Vector3 dir) {
            int numHits = Physics2D.RaycastNonAlloc(transform.position, dir, hits, rayLen, hitLayer);
            Debug.DrawRay(transform.position, dir * rayLen, numHits > 0 ? Color.green : Color.red);
            return numHits > 0 ? hits[0].normal : Vector2.zero;
        }

        private static int DeflectAngleIndex(int angleIndex, Vector2 hitNormal, Vector2 velocity) {
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
        }

        private static void SetPSysColorLifetime(ParticleSystem particleSys,
                                                 Gradient       gradient) {
            ParticleSystem.ColorOverLifetimeModule colOverLife = particleSys.colorOverLifetime;
            colOverLife.color = gradient;
        }
    }
}
