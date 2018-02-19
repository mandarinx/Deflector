using JetBrains.Annotations;
using UnityEngine;

namespace LunchGame01 {
    public class PlayerController : MonoBehaviour {

        [SerializeField]
        private GameObject             prefab;
        [SerializeField]
        private GameObjectSet          playerSet;
        [SerializeField]
        private SpawnPointSet          spawnPoints;

        private GameObjectPool<Player> players;

        private void Awake() {
            players = new GameObjectPool<Player>(parent: transform,
                                                 prefab: prefab,
                                                 size:   1,
                                                 grow:   false) {
                OnWillDespawn = p => {
                    p.transform.position = Vector3.left * 1000f;
                    p.Deactivate();
                }
            };
            players.Fill();
        }

        /// <summary>
        /// Despawns all players.
        /// Called by OnLevelExit handler.
        /// </summary>
        [UsedImplicitly]
        public void DeactivatePlayers() {
            players.Reset();
            playerSet.RemoveAll();
        }

        /// <summary>
        /// Positions all players on the playing field.
        /// Called by OnGameReady handler.
        /// </summary>
        [UsedImplicitly]
        public void ActivatePlayers() {
            for (int i = 0; i < players.NumElements; ++i) {
                Player p;
                players.Spawn(out p);
                playerSet.Add(p.gameObject);

                p.transform.position = spawnPoints.Count > 0
                    ? spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position
                    : Vector3.zero;
                p.Activate();
            }
        }
    }
}
