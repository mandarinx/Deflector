using GameEvents;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LunchGame01 {
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelExit : MonoBehaviour {

        [SerializeField]
        private Tile           openedDoor;
        [SerializeField]
        private Tile           closedDoor;

        [SerializeField]
        [HideInInspector]
        private GridLayout     grid;
        [SerializeField]
        [HideInInspector]
        private Tilemap        tilemap;

        [Header("Events Out")]
        [SerializeField]
        private GameEvent      onLevelExit;

        private BoxCollider2D  coll;

        private void OnEnable() {
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;
        }

        public void SetGridAndTilemap(GridLayout g, Tilemap t) {
            grid = g;
            tilemap = t;
        }

        public void OpenDoor() {
            coll.enabled = true;
            tilemap.SetTile(grid.WorldToCell(transform.position), openedDoor);
        }

        public void CloseDoor() {
            coll.enabled = false;
            tilemap.SetTile(grid.WorldToCell(transform.position), closedDoor);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // Disable the collider? then I'll have to enable it again sometime
            onLevelExit.Invoke();
        }
    }
}
