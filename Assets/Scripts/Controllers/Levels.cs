using System.Collections;
using GameEvents;
using UnityEngine;

public class Levels : MonoBehaviour {

    [SerializeField]
    private LevelSet   levelSet;
    [SerializeField]
    private LevelEvent onLevelLoaded;
    [SerializeField]
    private GameEvent  onLevelWillLoad;

    private int        curLevel;
    
    private void Start() {
        LoadNextLevel();
    }

    public void OnLevelExit() {
        levelSet[curLevel].Despawn();
        curLevel = (curLevel + 1) % levelSet.Count;
        LoadNextLevel();
    }

    private void LoadNextLevel() {
        onLevelWillLoad?.Invoke();

        Level level = levelSet[curLevel];
        level.Spawn();
        level.levelInstance.transform.localPosition = Vector3.left * 0.5f;

        StartCoroutine(DispatchOnLevelLoaded((level)));
    }

    private IEnumerator DispatchOnLevelLoaded(Level level) {
        yield return null;
        onLevelLoaded?.Invoke(level);
    }
}
