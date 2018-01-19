using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Lib {

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
        private UnityLevelEvent          onLevelLoaded;
        [SerializeField]
        private UnityEvent               onAllLevelsLoaded;
        private bool                     isLoading;
        private int                      numLoadedLevels;
        private int                      numLevelsToLoad;
        private List<LevelLoadOperation> loadOps;
        
        public float                     Progress { get; private set; }
        public int                       Count => loadOps.Count;

        private void Awake() {
            loadOps = new List<LevelLoadOperation>();
        }

        public void Load(int num, LoadSceneMode mode) {
            if (num >= levels.Count) {
                return;
            }
            
            StartLoadOp(num, mode);
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

            string sceneName = Path.GetFileNameWithoutExtension(levels[num].ScenePath);
            AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
            op.completed += asyncOp => { Resources.UnloadUnusedAssets(); };
        }

        private void StartLoadOp(int n, LoadSceneMode mode) {
            LevelLoadOperation loadOp = new LevelLoadOperation {
                scenePath = levels[n].ScenePath,
                level = levels[n]
            };
                
            // Scene path has not been set
            if (string.IsNullOrEmpty(loadOp.scenePath)) {
                return;
            }

            string sceneName = Path.GetFileNameWithoutExtension(loadOp.scenePath);
                
            // Scene is already loaded
            if (SceneManager.GetSceneByName(sceneName).IsValid()) {
                onLevelLoaded.Invoke(loadOp.level);
                return;
            }

            numLoadedLevels = 0;
            ++numLevelsToLoad;
            loadOp.asyncOp = SceneManager.LoadSceneAsync(sceneName, mode);
            loadOps.Add(loadOp);
            isLoading = numLevelsToLoad > 0;
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
