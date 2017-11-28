using UnityEngine;

public class Master : MonoBehaviour {

    public Turrets       turrets;
    public Player        player;
    public PlayerHealth  playerHealth;
    public GameObjectSet projectileSet;
    public GameObjectSet turretSet;

    private void Start() {
        playerHealth.onDead = OnPlayerDead;
    }

    private void OnPlayerDead() {
        player.Deactivate();
        turretSet.DespawnAll();
        projectileSet.DespawnAll();
    }

    public void OnGameReady() {
        player.Activate();
        turrets.StartInterval();
    }
}
