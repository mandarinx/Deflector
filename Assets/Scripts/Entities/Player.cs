using System.Collections;
using System.Collections.Generic;
using PowerTools;
using GameEvents;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Player : MonoBehaviour {
    
    public float                      moveSpeed = 1f;
    public float                      bounceForce;
    public int                        immuneBlinks;
    public float                      immuneBlinkDuration;
    public float                      footstepInterval;
    public float                      hurtDuration;
    public AnimationCurve             forceFalloff;
    public Transform                  shieldAnchor;
    public Shield                     shield;
    public SpriteAnim                 hitEffect;
    public SpriteAnim                 playerAnim;
    public PlayerHealth               playerHealth;
    public SpriteRenderer             blood;
    public SpriteRenderer             shadow;
    public GameEvent                  onFootstep;
    
    private Rigidbody2D               rb;
    private SpriteRenderer            sr;
    private float                     walkAngle;
    private int                       walkDir;
    private float                     hitTime;
    private Vector2                   hitNormal;
    private bool                      inputHit;
    private int                       inputMove;
    private bool                      activated;
    private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[8];
    private Coroutine                 hurtRoutine;
    private Collider2D                trigger;
    
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

    private void Awake() {
        activated = false;
        inputHit = false;
        inputMove = -1;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        walkAngle = Mathf.PI;
        hitEffect.gameObject.SetActive(false);
    }

    public void Activate() {
        gameObject.layer = LayerMask.NameToLayer("Player");
        blood.enabled = false;
        sr.enabled = true;
        sr.color = new Color(1f, 1f, 1f, 1f);
        shadow.color = new Color(1f, 1f, 1f, 1f);
        activated = true;
        trigger = null;
        StopAllCoroutines();
        hurtRoutine = null;
        playerAnim.Play(playerAnim.Clip);
        StartCoroutine(Footsteps());
    }

    public void Deactivate() {
        activated = false;
        playerAnim.Stop();
        StopAllCoroutines();
        hitEffect.gameObject.SetActive(false);
    }

    public void OnPlayerDied() {
        blood.enabled = true;
        sr.enabled = false;
        Deactivate();
    }

    public void Hit(Vector3 hitPos) {
        hitNormal = new Vector2(
            transform.position.x - hitPos.x, 
            transform.position.y - hitPos.y).normalized;
        Hit();
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
        
        // Shield

        if (Input.GetKeyDown(KeyCode.X)) {
            inputHit = true;
        }
        
        // Movement
        
        if (Input.GetKey(KeyCode.UpArrow)) {             inputMove = 2;
            if (Input.GetKey(KeyCode.RightArrow)) {      inputMove = 1; }
            else if (Input.GetKey(KeyCode.LeftArrow)) {  inputMove = 3; }
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {      inputMove = 6;
            if (Input.GetKey(KeyCode.RightArrow)) {      inputMove = 7; }
            else if (Input.GetKey(KeyCode.LeftArrow)) {  inputMove = 5; }
        }
        
        if (Input.GetKey(KeyCode.RightArrow)) {          inputMove = 0;
            if (Input.GetKey(KeyCode.UpArrow)) {         inputMove = 1; }
            else if (Input.GetKey(KeyCode.DownArrow)) {  inputMove = 7; }
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {      inputMove = 4;
            if (Input.GetKey(KeyCode.UpArrow)) {         inputMove = 3; }
            else if (Input.GetKey(KeyCode.DownArrow)) {  inputMove = 5; }
        }

        if (inputMove == 0 || inputMove == 1 || inputMove == 7) {
            sr.flipX = true;
        }
        if (inputMove == 3 || inputMove == 4 || inputMove == 5) {
            sr.flipX = false;
        }
        
        // Overlap triggers
        
        OverlapTrigger();
    }
    
    private void FixedUpdate() {
        
        // Shield

        if (inputHit) {
            inputHit = false;
            Projectile projectile = shield.GetOverlapped();
            if (projectile != null) {
                projectile.Hit(walkDir);
                hitEffect.gameObject.SetActive(true);
                hitEffect.Play(hitEffect.Clip);
                StartCoroutine(DisableHitEffect());
            }
        }
        
        // Movement
        
        Vector2 velocity = Vector2.zero;
        
        if (inputMove >= 0) {
            walkDir = inputMove;
            walkAngle = radianMap[inputMove];
            inputMove = -1;
            velocity = new Vector2 {
                x = Mathf.Cos(walkAngle) * moveSpeed,
                y = Mathf.Sin(walkAngle) * moveSpeed
            };
            shieldAnchor.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * walkAngle);
        }
        
        rb.MovePosition(rb.position + (velocity + (hitNormal * bounceForce)) * Time.fixedDeltaTime);
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

        if (other.gameObject.layer != LayerMask.NameToLayer("Projectiles")) {
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
        gameObject.layer = LayerMask.NameToLayer("Default");

        int count = 0;
        while (count < immuneBlinks) {
            sr.color = new Color(1f, 1f, 1f, 0f);
            shadow.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(immuneBlinkDuration);
            sr.color = new Color(1f, 1f, 1f, 1f);
            shadow.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(immuneBlinkDuration);
            ++count;
        }
        
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private IEnumerator DisableHitEffect() {
        yield return new WaitForSeconds(0.33f);
        hitEffect.gameObject.SetActive(false);
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
}
