using System.Collections;
using UnityEngine;

public class Turrets : MonoBehaviour {

    public GameObject prefab;
    public float spawnInterval = 4;
    public int top;
    public int right;
    public int bottom;
    public int left;
    
    private void Start () {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        while (true) {
            yield return new WaitForSeconds(spawnInterval);
            GameObject turret = Instantiate(prefab);
            float x = Random.Range(-left, right);
            float y = Random.Range(-bottom, top);
            turret.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(
            transform.position + (Vector3.up * top) + (Vector3.left  * left),
            transform.position + (Vector3.up * top) + (Vector3.right * right));
        Gizmos.DrawLine(
            transform.position + (Vector3.down * bottom) + (Vector3.left  * left),
            transform.position + (Vector3.down * bottom) + (Vector3.right * right));
        Gizmos.DrawLine(
            transform.position + (Vector3.up * top) + (Vector3.left  * left),
            transform.position + (Vector3.down * bottom) + (Vector3.left * left));
        Gizmos.DrawLine(
            transform.position + (Vector3.up * top) + (Vector3.right  * right),
            transform.position + (Vector3.down * bottom) + (Vector3.right * right));
    }
}
