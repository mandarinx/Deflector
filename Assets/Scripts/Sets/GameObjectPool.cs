using System.Collections.Generic;
using GameEvents;
using UnityEngine;

[CreateAssetMenu(menuName = "Pools/Game Object", fileName = "GameObjectPool.asset")]
public class GameObjectPool : ScriptableObject {

    public GameObject        prefab;
    public GameObjectEvent   onGameObjectAdded;
    public GameObjectEvent   onGameObjectRemoved;
    
    private List<GameObject> instances;

    public int Count => instances.Count;

    private void OnEnable() {
        instances = new List<GameObject>();
    }

    public void Spawn(Vector3 pos) {
        GameObject p = Instantiate(prefab);
        p.transform.position = pos;
        instances.Add(p);
        onGameObjectAdded?.Invoke(p);
    }

    public void Despawn(GameObject go) {
        if (!instances.Contains(go)) {
            return;
        }
        onGameObjectRemoved?.Invoke(go);
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
