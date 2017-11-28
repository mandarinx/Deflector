using System.Collections;
using System.Collections.Generic;
using PowerTools;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    
    public float          moveSpeed = 1f;
    public float          bounceForce;
    public AnimationCurve forceFalloff;
    public Transform      shieldAnchor;
    public Shield         shield;
    public SpriteAnim     hitEffect;
    
    private Rigidbody2D               rb;
    private float                     walkAngle;
    private int                       walkDir;
    private float                     hitTime;
    private Vector2                   hitNormal;
    private bool                      inputHit;
    private int                       inputMove;
    private readonly ContactPoint2D[] contactPoints = new ContactPoint2D[8];
    
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
        inputHit = false;
        inputMove = -1;
        rb = GetComponent<Rigidbody2D>();
        walkAngle = Mathf.PI;
        hitEffect.gameObject.SetActive(false);
    }

    private void Update() {

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
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Projectile")) {
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
        hitTime = Time.time;
    }

    private IEnumerator DisableHitEffect() {
        yield return new WaitForSeconds(0.33f);
        hitEffect.gameObject.SetActive(false);
    }
}
