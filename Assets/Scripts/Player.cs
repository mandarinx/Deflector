using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    
    public float moveSpeed = 1f;
    public float bounceForce;
    public AnimationCurve forceFalloff;
    
    private Rigidbody2D rb;
    private float walkAngle;
    private float hitTime;
    public Vector2 hitNormal;
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
        rb = GetComponent<Rigidbody2D>();
        walkAngle = Mathf.PI;
    }

    private void FixedUpdate() {
        int input = -1;
        
        if (Input.GetKey(KeyCode.UpArrow)) {             input = 0;
            if (Input.GetKey(KeyCode.RightArrow)) {      input = 1; }
            else if (Input.GetKey(KeyCode.LeftArrow)) {  input = 7; }
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {      input = 4;
            if (Input.GetKey(KeyCode.RightArrow)) {      input = 3; }
            else if (Input.GetKey(KeyCode.LeftArrow)) {  input = 5; }
        }
        
        if (Input.GetKey(KeyCode.RightArrow)) {          input = 2;
            if (Input.GetKey(KeyCode.UpArrow)) {         input = 1; }
            else if (Input.GetKey(KeyCode.DownArrow)) {  input = 3; }
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {      input = 6;
            if (Input.GetKey(KeyCode.UpArrow)) {         input = 7; }
            else if (Input.GetKey(KeyCode.DownArrow)) {  input = 5; }
        }
        
        Vector2 velocity = Vector2.zero;
        
        if (input >= 0) {
            walkAngle = radianMap[input];
            velocity = new Vector2 {
                x = Mathf.Sin(walkAngle) * moveSpeed,
                y = Mathf.Cos(walkAngle) * moveSpeed
            };
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

}
