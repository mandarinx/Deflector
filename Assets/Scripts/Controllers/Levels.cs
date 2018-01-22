using System.Collections;
using System.IO;
using GameEvents;
using Lib;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelLoader))]
public class Levels : MonoBehaviour {

    [SerializeField]
    private LevelEvent  onLevelLoaded;

    private int         curLevel;
    private LevelLoader levelLoader;

    private void Awake() {
        curLevel = -1;
        levelLoader = GetComponent<LevelLoader>();
    }
    
    public void UnloadCurrentLevel() {
        levelLoader.Unload(curLevel);
    }

    public void LoadNextLevel() {
        curLevel = (curLevel + 1) % levelLoader.Count;
        levelLoader.Load(curLevel, LoadSceneMode.Additive);
    }

    public void OnLevelLoaded(Level level) {
        string sceneName = Path.GetFileNameWithoutExtension(level.ScenePath);
        level.Prepare(SceneManager.GetSceneByName(sceneName));

        // Delay the level loaded event to allow the scene
        // objects to run all of their Awake and Start methods.
        StartCoroutine(DispatchOnLevelLoaded((level)));
    }

    private IEnumerator DispatchOnLevelLoaded(Level level) {
        yield return null;
        onLevelLoaded?.Invoke(level);
    }
}
