using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

namespace LunchGame01 {
    public class PlayerHearts : MonoBehaviour {

        [SerializeField]
        private HealthAsset   health;
        [SerializeField]
        private GameObject    heartPrefab;
        [SerializeField]
        private GameEvent     onHeartFilled;
        [SerializeField]
        private GameEvent     onHeartsRendered;

        private List<UIHeart> hearts;

        private void Awake() {
            health.onLivesChanged += OnLivesChanged;
            hearts = new List<UIHeart>(health.maxLives);

            for (int i = 0; i < health.maxLives; ++i) {
                AddHeart(false);
            }
        }

        public void Clear() {
            for (int i = 0; i < hearts.Count; ++i) {
                hearts[i].isAlive = false;
            }
        }

        public void RenderHearts() {
            StartCoroutine(RenderHeartsRoutine());
        }

        private IEnumerator RenderHeartsRoutine() {
            int numAlive = 0;
            for (int i = 0; i < hearts.Count; ++i) {
                numAlive += hearts[i].isAlive ? 1 : 0;
            }

            // Render hearts only when the number of alive
            // hearts is 0, ie. when the game starts or
            // resets.
            int heart = numAlive > 0 ? hearts.Count : 0;
            while (heart < hearts.Count) {
                onHeartFilled?.Invoke();
                yield return new WaitForSeconds(0.2f);
                hearts[heart].isAlive = heart < health.numLives;
                ++heart;
            }

            onHeartsRendered?.Invoke();
        }

        private void OnLivesChanged(int lives, int max) {
            // If player gets more health during playtime,
            // add more hearts
            if (max > hearts.Count) {
                AddHeart(true);
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
                hearts[i].isAlive = i < lives;
            }
        }

        private void AddHeart(bool alive) {
            GameObject instance = Instantiate(heartPrefab);
            instance.transform.SetParent(transform, false);
            UIHeart heart = instance.GetComponent<UIHeart>();
            hearts.Add(heart);
            heart.isAlive = alive;
        }
    }
}
