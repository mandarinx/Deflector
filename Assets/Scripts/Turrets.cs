using System.Collections;
using UnityEngine;

public class Turrets : MonoBehaviour {

    public float          spawnInterval = 4;
    public int            maxTurrets = -1;
    public int            top;
    public int            right;
    public int            bottom;
    public int            left;
    public GameObjectSet  spawnedTurrets;

    public void StartInterval() {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        while (maxTurrets < 0 || spawnedTurrets.Count < maxTurrets) {
            yield return new WaitForSeconds(spawnInterval);
            float x = Random.Range(-left, right);
            float y = Random.Range(-bottom, top);
            spawnedTurrets.Spawn(new Vector3(x + 0.5f, y + 0.5f, 0));
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(
            (Vector3.up * top) + (Vector3.left  * left),
            (Vector3.up * top) + (Vector3.right * right));
        Gizmos.DrawLine(
            (Vector3.down * bottom) + (Vector3.left  * left),
            (Vector3.down * bottom) + (Vector3.right * right));
        Gizmos.DrawLine(
            (Vector3.up * top) + (Vector3.left  * left),
            (Vector3.down * bottom) + (Vector3.left * left));
        Gizmos.DrawLine(
            (Vector3.up * top) + (Vector3.right  * right),
            (Vector3.down * bottom) + (Vector3.right * right));
    }
}
