using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class PlayerHealthListener : MonoBehaviour {

    public PlayerHealth health;
    public GameObject   heartPrefab;
    public GameEvent    onHeartFilled;

    private List<UIHeart> hearts;
    
    private void Awake() {
        health.onLivesChanged += OnLivesChanged;
        hearts = new List<UIHeart>(health.maxLives);
        
        for (int i = 0; i < health.maxLives; ++i) {
            AddHeart(false);
        }
    }

    public IEnumerator RenderHearts() {
        int heart = 0;
        while (heart < hearts.Count) {
            onHeartFilled?.Invoke();
            yield return new WaitForSeconds(0.2f);
            hearts[heart].isAlive = true;
            ++heart;
        }
    }
    
//    public void ResetHearts() {
//        for (int i = 0; i < hearts.Count; ++i) {
//            hearts[i].isAlive = false;
//        }
//    }

    private void OnLivesChanged(int lives, int max) {
        if (max > hearts.Count) {
            AddHeart(true);
        }
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
