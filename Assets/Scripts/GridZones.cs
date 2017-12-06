using System.Collections.Generic;
using UnityEngine;

public class GridZones : MonoBehaviour {

    public Grid              grid;
    public Rect[]            zones;

    private List<Vector2Int> coordinates = new List<Vector2Int>(128);

    public int numCoordinates => coordinates.Count;
    
    private void OnEnable() {
        Recalculate();
    }

    public void Recalculate() {
        coordinates.Clear();
        for (int i = 0; i < zones.Length; ++i) {
            Rect zone = zones[i];
            int wi = Mathf.FloorToInt(zone.width / grid.cellSize.x);
            int hi = Mathf.FloorToInt(zone.height / grid.cellSize.y);
            int x = 0;
            int y = 0;
            int px = (int)zone.position.x;
            int py = (int)zone.position.y;
            for (int n = 0; n < (wi * hi); ++n) {
                Vector3 world = grid.CellToWorld(new Vector3Int(px + x, py + y, 0));
                coordinates.Add(new Vector2Int((int)world.x, (int)world.y));
                x = (x + 1) % wi;
                y += x == 0 ? 1 : 0;
            }
        }
    }

    public Vector2Int GetCoordinate(int n) {
        return coordinates[n];
    }

    public Vector3 GetCoordinateWorld(int n) {
        return new Vector3(
            coordinates[n].x + 0.5f,
            coordinates[n].y + 0.5f,
            0f);
    }

    private void OnDrawGizmos() {
        foreach (Vector2Int coord in coordinates) {
            Gizmos.DrawSphere(new Vector3(
                coord.x + grid.cellSize.x * 0.5f, 
                coord.y + grid.cellSize.y * 0.5f,
                0f), 0.25f);
        }
    }
}
