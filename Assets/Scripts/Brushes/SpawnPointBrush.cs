using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brushes/Spawn Point", fileName = "SpawnPointBrush.asset")]
[CustomGridBrush(hideAssetInstances:false, hideDefaultInstance:true, defaultBrush:false, defaultName:"Spawn Point")]
public class SpawnPointBrush : LayerObjectBrush<SpawnPoint> {

    public Color         color;
    public override bool alwaysCreateOnPaint => true;

    private List<SpawnPoint> m_Selection;

    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position) {
        CreateObject(grid, position, m_Prefab);
        BrushUtility.SetDirty(activeObject);
    }

    public override void Select(GridLayout grid, GameObject layer, BoundsInt position) {
        base.Select(grid, layer, position);

        if (m_Selection == null)
            m_Selection = new List<SpawnPoint>();

        m_Selection.Clear();

        foreach (SpawnPoint spawnPoint in allObjects) {
            if (position.Contains(grid.WorldToCell(spawnPoint.transform.position))) {
                m_Selection.Add(spawnPoint);
            }
        }
    }

    public override void Move(GridLayout grid, GameObject layer, BoundsInt from, BoundsInt to) {
        foreach (SpawnPoint spawnPoint in m_Selection) {
            spawnPoint.transform.Translate(grid.CellToWorld(to.min) - grid.CellToWorld(from.min));
        }
    }
}
