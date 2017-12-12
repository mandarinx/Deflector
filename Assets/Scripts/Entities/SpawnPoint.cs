using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    
    [SerializeField]
    private SpawnPointSet set;
    
    private void Awake() {
        set.Add(this);
    }
}
