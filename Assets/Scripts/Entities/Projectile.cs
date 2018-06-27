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
        private int                       contacts;
        private int                       angleIndex;
        private State                     state;
        private Vector2                   hitNormal;
        private Rigidbody2D               rb;
        private ParticleSystem            ps;
        private CircleCollider2D          coll;
        private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[2];

        public bool                       IsExploding => (state & State.EXPLODING) > 0;
        public bool                       IsCharged   => (state & State.CHARGED)   > 0;
        public bool                       IsNeutral   => (state & State.NEUTRAL)   > 0;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            ps = GetComponent<ParticleSystem>();
            coll = GetComponent<CircleCollider2D>();
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
        }

        private Vector3 reflected;

        private void OnCollisionEnter2D(Collision2D collision) {
            HandleCollision(collision);
        }

        private void OnCollisionStay2D(Collision2D collision) {
            HandleCollision(collision);
        }

        private void HandleCollision(Collision2D collision) {
            if (!LayerMasks.LayerInMask(collision.gameObject.layer, hitLayer)) {
                return;
            }

            if (!GetHitNormal(collision, out hitNormal)) {
                return;
            }

            reflected = Vector3.Reflect(GetVelocity(), hitNormal);
            angleIndex = GetDiagonal(reflected);
            angle = ((angleIndex * 90f) + 45f) * Mathf.Deg2Rad;
            rb.position += GetVelocity() * Time.fixedDeltaTime * 2f;
        }

        private bool GetHitNormal(Collision2D collision, out Vector2 normal) {
            contacts = collision.GetContacts(contactPoints);
            if (contacts == 0) {
                normal = Vector2.zero;
                return false;
            }

            normal = Vector2.zero;
            for (int i = 0; i < contacts; ++i) {
                normal += contactPoints[i].normal;
            }
            normal.Normalize();
            return true;
        }

        private static int GetDiagonal(Vector3 dir) {
            float deg = (Mathf.Atan2(dir.y, dir.x) + Mathf.PI) * Mathf.Rad2Deg;
            if (deg >= 0 && deg < 90) {
                return 2;
            }
            if (deg >= 90 && deg < 180) {
                return 3;
            }
            if (deg >= 180 && deg < 270) {
                return 0;
            }
            // when deg >= 270
            return 1;
        }

        private Vector2 GetVelocity() {
            return Angles.GetDirection(angle).normalized * speed;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, reflected);

            Gizmos.color = Color.red;
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
