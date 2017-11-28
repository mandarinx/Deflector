using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerHealth", fileName = "PlayerHealth.asset")]
public class PlayerHealth : ScriptableObject {
    
    [SerializeField]
    private int lives;
    [SerializeField]
    private int livesMax;

    public Action<int, int> onLivesChanged;
    public Action           onDead;

    private void OnDisable() {
        lives = livesMax;
    }

    public void SetLives(int l) {
        lives = l;
        onLivesChanged.Invoke(lives, livesMax);
        if (lives <= 0) {
            onDead.Invoke();
        }
    }

    public int maxLives => livesMax;
    public int numLives => lives;
}
