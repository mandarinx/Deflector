using GameEvents;
using UnityEngine;

public class Levels : MonoBehaviour {

    [SerializeField]
    private GameObjectSet levelSet;
    [SerializeField]
    private LevelEvent    onLevelLoaded;
    [SerializeField]
    private GameEvent     onLevelWillLoad;

    private int           curLevel;
    private GameObject    curInstance;
    
    public void OnGameReady() {
        LoadNextLevel();
    }

    public void OnLevelExit() {
        Destroy(curInstance);
        LoadNextLevel();
    }

    private void LoadNextLevel() {
        onLevelWillLoad?.Invoke();
        
        curInstance = Instantiate(levelSet[++curLevel % levelSet.Count]);
        curInstance.transform.localPosition = Vector3.left * 0.5f;
        
        onLevelLoaded?.Invoke(curInstance.GetComponent<Level>());
    }
}
