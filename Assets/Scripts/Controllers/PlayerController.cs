using HyperGames;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private GameObject             prefab;
    [SerializeField]
    private SpawnPointSet          spawnPoints;

    private GameObjectPool<Player> players;

    private void Awake() {
        players = new GameObjectPool<Player>(transform, prefab, 1, false);
        players.Fill();
    }

    /// <summary>
    /// Spawns any missing players and puts them on random spawn points.
    /// Called by OnLevelLoaded handler.
    /// </summary>
    [UsedImplicitly]
    public void ResetPlayers() {
        // A difference between NumElements and NumSpawned means
        // that not all players have been spawned.
        // NumElements is the number of pooled players. In Awake, we cap the
        // pool size to 1, which prevents Fill() from spawning more than
        // one player.
        // NumSpawned is 0 when none of the player have been spawned.
        // At first run, the difference will be 1 - 0 = 1. On subsequent
        // runs both NumElements and NumSpawned are 1, which means the
        // difference is 0, and the for loop never runs.
        for (int i = 0; i < players.NumElements - players.NumSpawned; ++i) {
            Player p;
            players.Spawn(out p);
        }

        for (int i = 0; i < players.NumSpawned; ++i) {
            players[i].transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
            players[i].Activate();
        }
    }

    /// <summary>
    /// Despawns all players.
    /// Called by OnLevelExit handler.
    /// </summary>
    [UsedImplicitly]
    public void DeactivatePlayers() {
        players.Reset();
    }
}
