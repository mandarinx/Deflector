using GameEvents;
using UnityEngine;

public class Coords : MonoBehaviour {

    public Grid              grid;
    public GridCoordsHashSet coords;
    public Vector3IntEvent   onCoordFreed;
    public Vector3IntEvent   onCoordOccupied;
    
    public void OccupyCoord(Vector3 worldPos) {
        Vector3Int coord = grid.WorldToCell(worldPos);
        coords.Add(coord);
        onCoordOccupied.Invoke(coord);
    }

    public void FreeCoord(Vector3 worldPos) {
        Vector3Int coord = grid.WorldToCell(worldPos);
        coords.Remove(coord);
        onCoordFreed.Invoke(coord);
    }

    private void OnDrawGizmos() {
        if (coords == null) {
            return;
        }
        
        foreach (Vector3Int coord in coords) {
            Gizmos.DrawCube(grid.GetCellCenterWorld(coord), Vector3.one * 0.5f);
        }
    }
}
