using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameObjectSet", fileName = "GameObjectSet.asset")]
public class GameObjectSet : ScriptableObject {

    public GameObject        prefab;
    
    private List<GameObject> instances;

    public int Count => instances.Count;

    public void Spawn(Vector3 pos) {
        GameObject p = Instantiate(prefab);
        p.transform.position = pos;
        instances.Add(p);
    }

    public void DespawnAll() {
        for (int i = instances.Count - 1; i >= 0; --i) {
            Destroy(instances[i].gameObject);
        }
        instances.Clear();
    }
}
