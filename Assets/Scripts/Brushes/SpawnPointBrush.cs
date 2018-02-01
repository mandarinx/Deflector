using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brushes/Spawn Point", fileName = "SpawnPointBrush.asset")]
[CustomGridBrush(hideAssetInstances:  false,
                 hideDefaultInstance: true,
                 defaultBrush:        false,
                 defaultName:         "Spawn Point")]
public class SpawnPointBrush : GridBrushBase {

    [SerializeField]
    private string 		m_LayerName;
    [SerializeField]
    private GameObject  m_Prefab;
    [SerializeField]
    private Vector3 	m_PrefabOffset;
    [SerializeField]
    private Color       color;

    public Color        Color => color;
    public string       Layer => m_LayerName;

    private List<SpawnPoint> m_Selection;

    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position) {
        // Get all SpawnPoints in grid
        SpawnPoint[] spawnPoints = BrushUtility.GetRootGrid().GetComponentsInChildren<SpawnPoint>();
        for (int i=0; i<spawnPoints.Length; ++i) {
            // If a SpawnPoint already exists at the current position, exit
            if (position == grid.WorldToCell(spawnPoints[i].transform.position)) {
                return;
            }
        }

        BrushUtility.Instantiate<SpawnPoint>(
            m_Prefab,
            BrushUtility.GetWorldPos(grid, position + m_PrefabOffset),
            BrushUtility.GetLayer(m_LayerName));
    }

    public override void Select(GridLayout grid, GameObject layer, BoundsInt position) {
        base.Select(grid, layer, position);

        if (m_Selection == null) {
            m_Selection = new List<SpawnPoint>();
        }

        m_Selection.Clear();

        SpawnPoint[] spawnPoints = BrushUtility.GetRootGrid().GetComponentsInChildren<SpawnPoint>();
        for (int i=0; i<spawnPoints.Length; ++i) {
            if (position.Contains(grid.WorldToCell(spawnPoints[i].transform.position))) {
                m_Selection.Add(spawnPoints[i]);
            }
        }
    }

    public override void Move(GridLayout grid, GameObject layer, BoundsInt from, BoundsInt to) {
        for (int i=0; i<m_Selection.Count; ++i) {
            m_Selection[i].transform.Translate(grid.CellToWorld(to.min) - grid.CellToWorld(from.min));
        }
    }
}
