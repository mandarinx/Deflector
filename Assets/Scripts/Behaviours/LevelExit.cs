using GameEvents;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelExit : MonoBehaviour {

    public Vector3Int coordinate;
    public Tile       openDoor;
    public Tile       closedDoor;
    
    [Header("Events Out")]
    public GameEvent  onLevelExit;
    
    private Tilemap   tilemap;

    public void OnLevelLoaded(Level level) {
        tilemap = level.Layers.GetLayer("Walls");
        Assert.IsNotNull(tilemap, $"Could not get layer Walls from level {level.name}");
    }
    
    public void OpenDoor() {
        tilemap.SetTile(coordinate, openDoor);
    }

    public void CloseDoor() {
        tilemap.SetTile(coordinate, closedDoor);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        onLevelExit.Invoke();
    }
}
