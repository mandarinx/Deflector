using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentPool<T> : ScriptableObject where T : Component {

    public GameObject   prefab;
    private List<T>     instances;

    public int Count => instances.Count;

    private void OnEnable() {
        instances = new List<T>();
    }

    public void Spawn(Vector3 pos) {
        GameObject instance = Instantiate(prefab);
        instance.transform.position = pos;
        instances.Add(instance.GetComponent<T>());
    }

    public void Despawn(GameObject go) {
        T comp = go.GetComponent<T>();
        if (!instances.Contains(comp)) {
            return;
        }
        instances.Remove(comp);
        Destroy(go);
    }

    public void DespawnAll() {
        for (int i = instances.Count - 1; i >= 0; --i) {
            Destroy(instances[i].gameObject);
        }
        instances.Clear();
    }
}
