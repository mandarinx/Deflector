using GameEvents;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelExit : MonoBehaviour {

    public Vector3Int coordinate;
    public Tile       openDoor;
    public Tilemap    tilemap;
    
    [Header("Events Out")]
    public GameEvent  onLevelExit;
    
    public void ShowExit() {
        tilemap.SetTile(coordinate, openDoor);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        onLevelExit.Invoke();
    }
}
