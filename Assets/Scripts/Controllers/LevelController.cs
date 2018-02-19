using System.Collections;
using System.IO;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LunchGame01 {
    [RequireComponent(typeof(LevelLoader))]
    public class LevelController : MonoBehaviour {

        [SerializeField]
        private LevelEvent  onLevelLoaded;

        private int         curLevel;
        private LevelLoader levelLoader;

        private void Awake() {
            curLevel = -1;
            levelLoader = GetComponent<LevelLoader>();
        }

        public void LoadLevel(int n) {
            int nextLevel = Mathf.Clamp(n, 0, levelLoader.Count);
            if (curLevel != nextLevel &&
                curLevel >= 0) {
                levelLoader.Unload(curLevel);
            }
            curLevel = nextLevel;
            levelLoader.Load(curLevel, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Handler for onLoadNextLevel event.
        /// </summary>
        [UsedImplicitly]
        public void LoadNextLevel() {
            int nextLevel = (curLevel + 1) % levelLoader.Count;
            if (curLevel != nextLevel &&
                curLevel >= 0) {
                levelLoader.Unload(curLevel);
            }
            curLevel = nextLevel;
            levelLoader.Load(curLevel, LoadSceneMode.Additive);
        }

        public void OnWillUnloadLevel(Level level) {
            level.IncreasePlayCount();
        }

        /// <summary>
        /// Handler for LevelLoader.OnLevelLoaded
        /// </summary>
        /// <param name="level"></param>
        [UsedImplicitly]
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
}
