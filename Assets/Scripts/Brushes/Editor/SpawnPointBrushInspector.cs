using UnityEditor;
using UnityEngine;

namespace Deflector {
    [CustomEditor(typeof(SpawnPointBrush))]
    public class SpawnPointBrushInspector : BrushBaseInspector {

        private SpawnPointBrush brush => target as SpawnPointBrush;

        public void OnSceneGUI() {
            Grid grid = BrushUtility.GetRootGrid();
            SpawnPoint[] spawnPoints = BrushUtility
                                      .GetLayer(brush.Layer)
                                     ?.GetComponentsInChildren<SpawnPoint>();

            if (spawnPoints == null) {
                return;
            }

            BrushEditorUtility.BeginQuads(brush.Color);
            for (int i = 0; i < spawnPoints.Length; ++i) {
                SelectSpawnPoint(grid, spawnPoints[i]);
            }
            BrushEditorUtility.EndQuads();
        }

        private static void SelectSpawnPoint(Grid grid, SpawnPoint spawnPoint) {
            BrushEditorUtility.DrawQuadBatched(grid, grid.WorldToCell(spawnPoint.transform.position));
        }
    }
}
