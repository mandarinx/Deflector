using System.Collections;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class PlayerHealthListener : MonoBehaviour {

    public PlayerHealth health;
    public GameObject   heartPrefab;
    public GameEvent    onHeartAdded;
    
    private void Awake() {
        health.onLivesChanged = OnLivesChanged;
    }

    public IEnumerator LoadHearts() {
        int heart = 0;
        while (heart < health.maxLives) {
            GameObject uiHeart = Instantiate(heartPrefab);
            uiHeart.transform.SetParent(transform, false);
            onHeartAdded.Raise();
            uiHeart.GetComponent<UIHeart>().isAlive = false;
            yield return new WaitForSeconds(0.2f);
            uiHeart.GetComponent<UIHeart>().isAlive = true;
            yield return new WaitForSeconds(0.2f);
            ++heart;
        }
    }

    public void ResetHearts() {
        for (int i = transform.childCount - 1; i >= 0; --i) {
            Destroy(transform.GetChild(i).gameObject);
        }
        health.SetLives(health.maxLives);
        StartCoroutine(LoadHearts());
    }

    private void OnLivesChanged(int lives, int max) {
        for (int i = 0; i < max; ++i) {
            transform.GetChild(i).GetComponent<UIHeart>().isAlive = i < lives;
        }
    }
}
