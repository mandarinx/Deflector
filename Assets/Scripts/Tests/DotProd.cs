using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DotProd : MonoBehaviour {

    public Transform target;

    public Vector3 normal;

    private void Update() {
        Vector3 input = target.position - transform.position;
        float dot = Vector3.Dot(normal.normalized, input.normalized);
        //  1 = same direction
        // -1 = opposite direction
        //  0 = perpendicular
//        Debug.Log(dot);
        
        Vector3 cross = Vector3.Cross(normal, input);
        Debug.DrawRay(transform.position, cross, Color.cyan);
        
        // Bounce back
        if (dot > 0.995f) {
            Debug.DrawRay(transform.position, input * 4f, Color.yellow);
        }
        
        // Bounce to either side
        if (dot <= 0.995f && dot > 0f) {
            // Right
            if (cross.normalized == Vector3.back) {
                Debug.DrawRay(transform.position, Vector3.Cross(input, Vector3.back).normalized * 4f, Color.yellow);
            }
            // Left
            if (cross.normalized == Vector3.forward) {
                Debug.DrawRay(transform.position, Vector3.Cross(input, Vector3.forward).normalized * 4f, Color.yellow);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, normal);
    }
}
