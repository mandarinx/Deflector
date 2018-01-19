using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level")]
[Serializable]
public class Level : ScriptableObject {
    
    [SerializeField]
    private string     scenePath;
    [SerializeField]
    private GameMode[] gameModes;
    
    // Since scene is not serialized, it should be reset upon leaving play mode
    public Scene       Scene { get; private set; }
    public Layers      Layers { get; private set; }
    public Grid        Grid { get; private set; }

    public int         NumGameModes => gameModes.Length;
    public string      ScenePath => scenePath;
    
    public GameMode GetGameMode(int n) {
        return gameModes[n];
    }

    public void Prepare(Scene scene) {
        Scene = scene;

        GameObject[] rootObjects = Scene.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; ++i) {
            Layers layers = rootObjects[i].GetComponent<Layers>();
            if (layers != null) {
                Layers = layers;
            }

            Grid grid = rootObjects[i].GetComponent<Grid>();
            if (grid != null) {
                Grid = grid;
            }
        }
    }
}
