using System.Collections;
using UnityEngine;

public class Turrets : MonoBehaviour {
    
    public float          spawnInterval = 4;
    public int            maxTurrets = -1;
    public SpawnPointSet  spawnPoints;
    public GameObjectSet  turrets;

    public void Activate() {
        StartCoroutine(Spawn());
    }

    public void Deactivate() {
        StopAllCoroutines();
    }

    private IEnumerator Spawn() {
        while (maxTurrets < 0 || turrets.Count < maxTurrets) {
            yield return new WaitForSeconds(spawnInterval);
            if (spawnPoints.Count > 0) {
                turrets.Spawn(spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position);
            }
        }
    }
}
