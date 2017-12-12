using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(SpawnPointBrush))]
public class SpawnPointBrushInspector : LayerObjectBrushEditor<SpawnPoint> {
    
    public void OnSceneGUI() {
        Transform layer = brush.GetLayer();
        Tilemap tilemap = brush.GetLayer().GetComponent<Tilemap>();
        BrushEditorUtility.BeginQuads((target as SpawnPointBrush).color);
        foreach (Transform cell in layer) {
            if (cell.GetComponent<SpawnPoint>() == null) {
                continue;
            }
            BrushEditorUtility.DrawQuadBatched(tilemap.layoutGrid, tilemap.WorldToCell(cell.position));
        }
        BrushEditorUtility.EndQuads();
    }
}
