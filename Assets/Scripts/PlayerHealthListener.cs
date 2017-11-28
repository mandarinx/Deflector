using System.Collections;
using UnityEngine;

public class PlayerHealthListener : MonoBehaviour {

    public PlayerHealth health;
    public GameObject   heartPrefab;
    
    private void Awake() {
        health.onLivesChanged = OnLivesChanged;
    }

    public IEnumerator LoadHearts() {
        int heart = 0;
        while (heart < health.maxLives) {
            GameObject uiHeart = Instantiate(heartPrefab);
            uiHeart.transform.SetParent(transform, false);
            uiHeart.GetComponent<UIHeart>().isAlive = false;
            yield return new WaitForSeconds(0.2f);
            uiHeart.GetComponent<UIHeart>().isAlive = true;
            yield return new WaitForSeconds(0.2f);
            ++heart;
        }
    }

    private void OnLivesChanged(int lives, int max) {
        for (int i = 0; i < max; ++i) {
            transform.GetChild(i).GetComponent<UIHeart>().isAlive = i < lives;
        }
    }
}
