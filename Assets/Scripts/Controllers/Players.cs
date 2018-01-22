using UnityEngine;

public class Players : MonoBehaviour {

    public GameObject    prefab;
    public SpawnPointSet spawnPoints;

    private Player[] players;

    private void Awake() {
        players = new Player[1];
    }

    public void ResetPlayers() {
        for (int i = 0; i < players.Length; ++i) {
            if (players[i] == null) {
                players[i] = Instantiate(prefab).GetComponent<Player>();
            }

            players[i].transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
            players[i].Activate();
        }
    }
}
