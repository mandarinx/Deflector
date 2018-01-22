using System;
using UnityEngine;

[Serializable]
public class SpawnPoint : MonoBehaviour {
    
    [SerializeField]
    private SpawnPointSet set;
    
    private void Start() {
        set.Add(this);
    }
}
