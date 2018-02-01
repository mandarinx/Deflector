using System;
using UnityEngine;

[Serializable]
public class SpawnPoint : MonoBehaviour {

    [SerializeField]
    private SpawnPointSet set;

    private void Start() {
        Debug.Log($"Add {name} to {set.name}");
        set.Add(this);
    }
}
