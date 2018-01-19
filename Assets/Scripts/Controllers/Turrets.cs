using System.Collections;
using UnityEngine;

public class Turrets : MonoBehaviour {
    
    public float          spawnInterval = 4;
    public int            maxTurrets = -1;
    public SpawnPointSet  spawnPoints;
    public GameObjectPool turrets;

    public void Activate() {
        StartCoroutine(Spawn());
    }

    public void Deactivate() {
        StopAllCoroutines();
    }

    private IEnumerator Spawn() {
//        Debug.Log($"Turrets.Spawn maxTurrets: {maxTurrets} turrets: {turrets.Count} spawnPoints: {spawnPoints.Count}");
        while (maxTurrets < 0 || turrets.Count < maxTurrets) {
//            Debug.Log($"Wait for {spawnInterval}");
            yield return new WaitForSeconds(spawnInterval);
//            Debug.Log($"Spawn turret? {spawnPoints.Count > 0}");
            if (spawnPoints.Count > 0) {
                turrets.Spawn(transform, spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position);
            }
        }
    }
}
