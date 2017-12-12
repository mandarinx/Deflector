using System.Collections;
using UnityEngine;

public class Turrets : MonoBehaviour {

    public float          spawnInterval = 4;
    public int            maxTurrets = -1;
    public SpawnPointSet  spawnPoints;
    public GameObjectSet  spawnedTurrets;

    public void Activate() {
        StartCoroutine(Spawn());
    }

    public void Deactivate() {
        StopAllCoroutines();
    }

    private IEnumerator Spawn() {
        while (maxTurrets < 0 || spawnedTurrets.Count < maxTurrets) {
            yield return new WaitForSeconds(spawnInterval);
            spawnedTurrets.Spawn(spawnPoints.Get(Random.Range(0, spawnPoints.Count)).transform.position);
        }
    }
}
