using System.Collections;
using System.Collections.Generic;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

namespace LunchGame01 {
    public class PlayerPaddles : MonoBehaviour {

        [SerializeField]
        private HealthAsset   health;
        [SerializeField]
        private GameObject    paddlePrefab;
        [SerializeField]
        private GameEvent     onPaddleFilled;
        [SerializeField]
        private GameEvent     onPaddlesRendered;

        private List<UIPaddle> hearts;

        private void Awake() {
            health.onLivesChanged += OnLivesChanged;
            hearts = new List<UIPaddle>(health.maxLives);

            for (int i = 0; i < health.maxLives; ++i) {
                UIPaddle paddle = AddPaddle();
                paddle.isFull = false;
            }
        }

        /// <summary>
        /// Resets all paddles to ... dead?
        /// Called by OnGameReset
        /// </summary>
        [UsedImplicitly]
        public void Clear() {
            for (int i = 0; i < hearts.Count; ++i) {
                hearts[i].isFull = false;
            }
        }

        public void RenderPaddles() {
            StartCoroutine(RenderHeartsRoutine());
        }

        private IEnumerator RenderHeartsRoutine() {
            int numAlive = 0;
            for (int i = 0; i < hearts.Count; ++i) {
                numAlive += hearts[i].isFull ? 1 : 0;
            }

            // Render hearts only when the number of alive
            // hearts is 0, ie. when the game starts or
            // resets.
            int heart = numAlive > 0 ? hearts.Count : 0;
            while (heart < hearts.Count) {
                onPaddleFilled?.Invoke();
                yield return new WaitForSeconds(0.2f);
                hearts[heart].isFull = heart < health.numLives;
                ++heart;
            }

            onPaddlesRendered?.Invoke();
        }

        private void OnLivesChanged(int lives, int max) {
            // If player gets more health during playtime,
            // add more hearts
            if (max > hearts.Count) {
                UIPaddle paddle = AddPaddle();
                paddle.isFull = true;
            }

            // If player loses hearts during playtime,
            // remove hearts
            if (max < hearts.Count) {
                int remove = hearts.Count - max;
                hearts.RemoveRange(max, remove);
                for (int i = 0; i < remove; ++i) {
                    Destroy(transform.GetChild(max));
                }
            }

            for (int i = 0; i < max; ++i) {
                hearts[i].isFull = i < lives;
            }
        }

        private UIPaddle AddPaddle() {
            GameObject instance = Instantiate(paddlePrefab);
            instance.transform.SetParent(transform, false);
            UIPaddle paddle = instance.GetComponent<UIPaddle>();
            hearts.Add(paddle);
            return paddle;
        }
    }
}
