using System;
using GameEvents;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player Health", fileName = "PlayerHealth.asset")]
public class PlayerHealth : ScriptableObject {
    
    [SerializeField]
    private int       lives;
    [SerializeField]
    private int       livesMax;
    [SerializeField]
    private GameEvent onDead;
    [SerializeField]
    private GameEvent onLostLife;

    public Action<int, int> onLivesChanged;

    private void OnDisable() {
        lives = livesMax;
    }

    public void SetLives(int l) {
        lives = l;
        lives = Mathf.Clamp(lives, 0, livesMax);
        onLivesChanged?.Invoke(lives, livesMax);
    }

    public void AddLives(int num) {
        lives += num;
        lives = Mathf.Min(lives, livesMax);
        onLivesChanged?.Invoke(lives, livesMax);
    }

    public void RemoveLives(int num) {
        lives -= num;
        lives = Mathf.Max(lives, 0);
        onLivesChanged?.Invoke(lives, livesMax);
        onLostLife?.Invoke();
        if (lives <= 0) {
            onDead?.Invoke();
        }
    }

    public void SetMaxLives(int m) {
        livesMax = m;
        lives = m;
        onLivesChanged?.Invoke(lives, livesMax);
    }

    public int maxLives => livesMax;
    public int numLives => lives;
}
