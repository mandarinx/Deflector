using System.Collections;
using System.Collections.Generic;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour {

        public float                      moveSpeed = 1f;
        public float                      bounceForce;
        public int                        immuneBlinks;
        public float                      immuneBlinkDuration;
        public float                      footstepInterval;
        public float                      hurtDuration;
        public LayerMask                  hurtBy;
        public AnimationCurve             forceFalloff;
        public Transform                  swordAnchor;
        public HitPoint                   hitPoint;

        [Space]
        [Header("Animations")]
        [SerializeField]
        private SpriteAnimPlayer          animPlayer;
        [SerializeField]
        private AnimationClip             animIdle;
        [SerializeField]
        private AnimationClip             animHit;

        [Space]
        public HealthAsset                playerHealth;
        public SpriteRenderer             shadow;
        public GameEvent                  onFootstep;
        public Vector3Event               onDiedAt;
        public GameEvent                  onPaddleSwing;

        private Rigidbody2D               rb;
        private SpriteRenderer            sr;
        private Color                     shadowColor;
        private float                     walkAngle;
        private int                       walkDir;
        private float                     hitTime;
        private Vector2                   hitNormal;
        private int                       inputMove;
        private bool                      activated;
        private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[8];
        private Coroutine                 hurtRoutine;
        private Collider2D                trigger;

        private const uint up    = 0x01 << 0;
        private const uint right = 0x01 << 1;
        private const uint down  = 0x01 << 2;
        private const uint left  = 0x01 << 3;
        private static readonly Dictionary<uint, int> inputDirections = new Dictionary<uint, int> {
            { right,        0 },
            { right | up,   1 },
            { up,           2 },
            { up | left,    3 },
            { left,         4 },
            { left | down,  5 },
            { down,         6 },
            { down | right, 7 },
        };
        private static readonly Dictionary<int, bool> flipDirections = new Dictionary<int, bool> {
            { 0, false },
            { 1, false },
            { 2, false },
            { 3, true },
            { 4, true },
            { 5, true },
            { 6, false },
            { 7, false },
        };

        public Vector2 Velocity { get; private set; }

        private void Awake() {
            activated = false;
            inputMove = -1;
            rb = GetComponent<Rigidbody2D>();
            sr = animPlayer.GetComponent<SpriteRenderer>();
            walkAngle = Mathf.PI;
            shadowColor = shadow.color;
            animPlayer.OnDone(OnAnimDone);
        }

        [ContextMenu("Activate")]
        public void Activate() {
            gameObject.layer = LayerMask.NameToLayer("Player");
            hitPoint.Show();
            sr.enabled = true;
            sr.color = new Color(1f, 1f, 1f, 1f);
            shadow.enabled = true;
            shadow.color = shadowColor;
            activated = true;
            trigger = null;
            StopAllCoroutines();
            hurtRoutine = null;
            animPlayer.Play(animIdle);
            StartCoroutine(Footsteps());
        }

        public void Deactivate() {
            activated = false;
            animPlayer.Stop();
            StopAllCoroutines();
        }

        /// <summary>
        /// Called by the GameEventLister for onPlayerDied
        /// </summary>
        [UsedImplicitly]
        public void OnPlayerDied() {
            sr.enabled = false;
            shadow.enabled = false;
            hitPoint.Hide();
            Deactivate();
            onDiedAt.Invoke(transform.position);
        }

        private void OnAnimDone(AnimationClip clip) {
            if (clip.name.Equals("PlayerIdle")) {
                return;
            }
            animPlayer.Play(animIdle);
        }

        /// <summary>
        /// Called by the Killable component
        /// </summary>
        /// <param name="hitPos">Position of the hit</param>
        [UsedImplicitly]
        public void Hit(Vector3 hitPos) {
            hitNormal = new Vector2(
                transform.position.x - hitPos.x,
                transform.position.y - hitPos.y).normalized;
            Hit();
        }

        public void SetImmune(bool immune) {
            gameObject.layer = LayerMask.NameToLayer(immune ? "Default" : "Player");
        }

        private void Hit() {
            playerHealth.RemoveLives(1);
            hitTime = Time.time;
            StartCoroutine(Immune());
        }

        private void Update() {
            if (!activated) {
                return;
            }

            // HitPoint

            if (Input.GetKeyDown(KeyCode.X)) {
                animPlayer.Play(animHit);
                hitPoint.Hit(walkDir);
                if (onPaddleSwing != null) {
                    onPaddleSwing.Invoke();
                }
            }

            // Movement

            inputMove = GetInputDirection(GetInputMask());
            if (inputMove > -1) {
                sr.flipX = flipDirections[inputMove];
            }

            // Overlap triggers

            OverlapTrigger();
        }

        private void FixedUpdate() {
            Velocity = Vector2.zero;

            if (inputMove >= 0) {
                walkDir = inputMove;
                walkAngle = Angles.GetAngle(inputMove);
                inputMove = -1;
                Velocity = Angles.GetDirection(walkAngle) * moveSpeed;
                swordAnchor.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * walkAngle);
            }

            rb.MovePosition(rb.position + (Velocity + (hitNormal * bounceForce)) * Time.fixedDeltaTime);
            hitNormal *= forceFalloff.Evaluate(Mathf.Clamp01((Time.time - hitTime) / 1f));
        }

        private void OnTriggerEnter2D(Collider2D other) {
            trigger = other;
        }

        private void OnTriggerExit2D(Collider2D other) {
            trigger = null;
        }

        private void OverlapTrigger() {
            if (trigger == null) {
                return;
            }

            if (hurtRoutine != null) {
                return;
            }

            Hurt hurt = trigger.GetComponent<Hurt>();
            if (hurt == null) {
                return;
            }

            playerHealth.RemoveLives(1);
            hurtRoutine = StartCoroutine(Hurt());
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!activated) {
                return;
            }

            if (!LayerMasks.LayerInMask(other.gameObject.layer, hurtBy)) {
                return;
            }

            int contacts = other.GetContacts(contactPoints);
            if (contacts == 0) {
                return;
            }

            hitNormal = Vector2.zero;
            for (int i = 0; i < contacts; ++i) {
                hitNormal += contactPoints[i].normal;
            }
            hitNormal /= contacts;
            hitNormal.Normalize();

            Hit();
        }

        private IEnumerator Hurt() {
            float startTime = Time.time;
            float red = 0f;
            while (Time.time - startTime < hurtDuration) {
                sr.color = new Color(1f, red, red, 1f);
                red += Time.deltaTime / hurtDuration;
                yield return null;
            }
            sr.color = new Color(1f, 1f, 1f, 1f);
            hurtRoutine = null;
        }

        private IEnumerator Immune() {
            SetImmune(true);

            int count = 0;
            while (count < immuneBlinks) {
                sr.color = new Color(1f, 1f, 1f, 0f);
                shadow.color = new Color(1f, 1f, 1f, 0f);
                yield return new WaitForSeconds(immuneBlinkDuration);
                sr.color = new Color(1f, 1f, 1f, 1f);
                shadow.color = shadowColor;
                yield return new WaitForSeconds(immuneBlinkDuration);
                ++count;
            }

            SetImmune(false);
        }

        private IEnumerator Footsteps() {
            float stepTime = -1f;
            while (activated) {
                if (inputMove < 0) {
                    stepTime = -1f;
                }
                else if (stepTime < 0f ||
                         Time.time - stepTime >= footstepInterval) {
                    stepTime = Time.time;
                    onFootstep.Invoke();
                }
                yield return null;
            }
        }

        private static uint GetInputMask() {
            uint mask = 0;
            mask |= (uint)((Input.GetKey(KeyCode.UpArrow)    ? 1 : 0) << 0);
            mask |= (uint)((Input.GetKey(KeyCode.RightArrow) ? 1 : 0) << 1);
            mask |= (uint)((Input.GetKey(KeyCode.DownArrow)  ? 1 : 0) << 2);
            mask |= (uint)((Input.GetKey(KeyCode.LeftArrow)  ? 1 : 0) << 3);
            return mask;
        }

        private static int GetInputDirection(uint inputMask) {
            int dir;
            return inputDirections.TryGetValue(inputMask, out dir) ? dir : -1;
        }
    }
}
