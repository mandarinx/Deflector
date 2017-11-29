using System;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerHealth", fileName = "PlayerHealth.asset")]
public class PlayerHealth : ScriptableObject {
    
    [SerializeField]
    private int       lives;
    [SerializeField]
    private int       livesMax;
    [SerializeField]
    private GameEvent onDead;

    public Action<int, int> onLivesChanged;

    private void OnDisable() {
        lives = livesMax;
    }

    public void SetLives(int l) {
        lives = l;
        onLivesChanged.Invoke(lives, livesMax);
        if (lives <= 0) {
            onDead.Raise();
        }
    }

    public int maxLives => livesMax;
    public int numLives => lives;
}
