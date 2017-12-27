using GameEvents;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelExit : MonoBehaviour {

    public Vector3Int coordinate;
    public Tile       openDoor;
    
    [Header("Events Out")]
    public GameEvent  onLevelExit;
    
    private Tilemap    tilemap;

    public void OnLevelLoaded(Level level) {
        tilemap = level.GetLayer("Walls");
    }
    
    public void ShowExit() {
        tilemap.SetTile(coordinate, openDoor);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        onLevelExit.Invoke();
    }
}
