using System.Collections.Generic;
using GameEvents;
using UnityEngine;

[CreateAssetMenu(menuName = "Pools/Game Object", fileName = "GameObjectPool.asset")]
public class GameObjectPool : ScriptableObject {

    public GameObject        prefab;
    public GameObjectEvent   onSpawned;
    public GameObjectEvent   onDespawned;
    
    private List<GameObject> instances;

    public int Count => instances.Count;

    private void OnEnable() {
        instances = new List<GameObject>();
    }

    public void Spawn(Transform parent, Vector3 pos) {
        GameObject p = Instantiate(prefab);
        p.transform.SetParent(parent, false);
        p.transform.position = pos;
        instances.Add(p);
        onSpawned?.Invoke(p);
    }

    public void Despawn(GameObject go) {
        if (!instances.Contains(go)) {
            return;
        }
        onDespawned?.Invoke(go);
        Destroy(go);
        instances.Remove(go);
    }

    public void DespawnAll() {
        for (int i = instances.Count - 1; i >= 0; --i) {
            Destroy(instances[i].gameObject);
        }
        instances.Clear();
    }
}
