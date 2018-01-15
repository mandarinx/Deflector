using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Level")]
public class Level : ScriptableObject {
    
    [SerializeField]
    private GameObject levelPrefab;
    [SerializeField]
    private GameMode   gameMode;

    public GameObject  levelInstance { get; private set; }
    public GameMode    mode => gameMode;

    private void OnEnable() {
        Despawn();
        levelInstance = null;
    }

    public void Spawn() {
        levelInstance = Instantiate(levelPrefab);
    }

    public void Despawn() {
        #if UNITY_EDITOR
        DestroyImmediate(levelInstance);
        #else
        Destroy(levelInstance);
        #endif
    }

    public Tilemap GetLayer(string layer) {
        return levelInstance.GetComponent<Layers>()?.GetLayer(layer);
    }
}
