﻿using System.Collections;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deflector {
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

        /// <summary>
        /// Handler for onGameReset
        /// </summary>
        [UsedImplicitly]
        public void ResetAndUnloadCurrentLevel() {
            if (curLevel >= 0) {
                levelLoader.Unload(curLevel);
            }
            curLevel = -1;
        }

        [UsedImplicitly]
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

        /// <summary>
        /// Handler for LevelLoader.OnLevelLoaded
        /// </summary>
        /// <param name="level"></param>
        [UsedImplicitly]
        public void OnLevelLoaded(Level level) {
            // Delay the level loaded event to allow the scene
            // objects to run all of their Awake and Start methods.
            StartCoroutine(DispatchOnLevelLoaded((level)));
        }

        private IEnumerator DispatchOnLevelLoaded(Level level) {
            yield return null;
            if (onLevelLoaded != null) {
                onLevelLoaded.Invoke(level);
            }
        }
    }
}
