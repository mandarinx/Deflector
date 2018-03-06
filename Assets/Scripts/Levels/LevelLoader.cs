using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LunchGame01 {

    [Serializable]
    public class UnityLevelEvent : UnityEvent<Level> {}

    [Serializable]
    public class LevelLoadOperation {
        public string         scenePath = "";
        public Level          level;
        [HideInInspector]
        public AsyncOperation asyncOp;
    }

    [AddComponentMenu("Lib/Level Loader")]
    public class LevelLoader : MonoBehaviour {

        [SerializeField]
        private LevelSet                 levels;
        [SerializeField]
        private UnityEvent               onWillLoad;
        [SerializeField]
        private UnityLevelEvent          onLevelLoaded;
        [SerializeField]
        private UnityEvent               onAllLevelsLoaded;
        [SerializeField]
        private UnityLevelEvent          onWillUnload;
        [SerializeField]
        private UnityEvent               onUnloaded;
        private bool                     isLoading;
        private int                      numLoadedLevels;
        private int                      numLevelsToLoad;
        private List<LevelLoadOperation> loadOps;

        public float                     Progress { get; private set; }
        public int                       Count => levels.Count;

        private void Awake() {
            loadOps = new List<LevelLoadOperation>();
        }

        public void ResetLevels() {
            for (int i = 0; i < levels.Count; ++i) {
                levels[i].Reset();
            }
        }

        public bool Load(int num, LoadSceneMode mode) {
            if (num >= levels.Count) {
                return false;
            }

            return StartLoadOp(num, mode);
        }

        public void LoadAll(LoadSceneMode mode) {
            for (int i = 0; i < loadOps.Count; ++i) {
                StartLoadOp(i, mode);
            }

            if (!isLoading) {
                onAllLevelsLoaded.Invoke();
            }
        }

        public void Unload(int num) {
            if (num >= levels.Count) {
                return;
            }

            onWillUnload.Invoke(levels[num]);
            SceneManager
                .UnloadSceneAsync(Path.GetFileNameWithoutExtension(levels[num].ScenePath))
                .completed += OnLevelUnloaded;
        }

        private void OnLevelUnloaded(AsyncOperation op) {
            Resources.UnloadUnusedAssets();
            onUnloaded.Invoke();
        }

        private bool StartLoadOp(int n, LoadSceneMode mode) {
            LevelLoadOperation loadOp = new LevelLoadOperation {
                scenePath = levels[n].ScenePath,
                level = levels[n]
            };

            // Scene path has not been set
            if (string.IsNullOrEmpty(loadOp.scenePath)) {
                return false;
            }

            onWillLoad.Invoke();
            string sceneName = Path.GetFileNameWithoutExtension(loadOp.scenePath);

            // Scene is already loaded
            if (SceneManager.GetSceneByName(sceneName).IsValid()) {
                onLevelLoaded.Invoke(loadOp.level);
                return false;
            }

            numLoadedLevels = 0;
            ++numLevelsToLoad;
            loadOp.asyncOp = SceneManager.LoadSceneAsync(sceneName, mode);
            loadOps.Add(loadOp);
            isLoading = numLevelsToLoad > 0;

            return true;
        }

        private void Update() {
            if (!isLoading) {
                return;
            }

            for (int i = loadOps.Count - 1; i >= 0; --i) {
                LevelLoadOperation loadOp = loadOps[i];

                if (loadOp.asyncOp == null ||
                    !loadOp.asyncOp.isDone) {
                    continue;
                }

                onLevelLoaded.Invoke(loadOp.level);
                loadOps.RemoveAt(i);
                ++numLoadedLevels;
            }

            Progress = (float)numLoadedLevels / numLevelsToLoad;

            if (numLoadedLevels < numLevelsToLoad) {
                return;
            }

            onAllLevelsLoaded.Invoke();
            numLevelsToLoad = 0;
            numLoadedLevels = 0;
            isLoading = false;
        }

        public void OnSceneLoaded(UnityAction<Level> listener) {
            onLevelLoaded.AddListener(listener);
        }

        public void OnAllScenesLoaded(UnityAction listener) {
            onAllLevelsLoaded.AddListener(listener);
        }
    }
}
