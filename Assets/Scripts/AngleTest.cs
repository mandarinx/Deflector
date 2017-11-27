using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AngleTest : MonoBehaviour {

    [Range(0f, Mathf.PI * 2)]
    public float angle;

    private void OnDrawGizmos() {
        Vector3 velocity = new Vector3(Mathf.Cos(angle) * 2f, Mathf.Sin(angle) * 2f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, velocity);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + velocity);
    }
}
