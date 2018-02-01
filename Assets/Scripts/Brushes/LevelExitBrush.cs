using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Brushes/Level Exit", fileName = "LevelExitBrush.asset")]
[CustomGridBrush(hideAssetInstances:  false,
                 hideDefaultInstance: true,
                 defaultBrush:        false,
                 defaultName:         "Level Exit")]
public class LevelExitBrush : GridBrushBase {

    [SerializeField]
    private string 		    m_LayerName;
    [SerializeField]
    private GameObject      m_Prefab;
    [SerializeField]
    private Vector3 	    m_PrefabOffset;

    private List<LevelExit> m_Selection;

    public string           Layer => m_LayerName;

    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position) {
        // Get all LevelExits in grid
        LevelExit[] levelExits = BrushUtility.GetRootGrid().GetComponentsInChildren<LevelExit>();
        for (int i=0; i<levelExits.Length; ++i) {
            // If a LevelExit already exists at the current position, exit
            if (position == grid.WorldToCell(levelExits[i].transform.position)) {
                return;
            }
        }

        LevelExit le = BrushUtility.Instantiate<LevelExit>(
            m_Prefab,
            BrushUtility.GetWorldPos(grid, position + m_PrefabOffset),
            BrushUtility.GetLayer(m_LayerName));

        Tilemap tm = BrushUtility
           .GetLayer(m_LayerName)
           .GetComponent<Tilemap>();

        if (tm == null) {
            return;
        }

        BrushUtility.RegisterUndo(tm, $"Clear tile");
        tm.SetTile(grid.WorldToCell(le.transform.position), null);
    }

    public override void Select(GridLayout grid, GameObject layer, BoundsInt position) {
        base.Select(grid, layer, position);

        if (m_Selection == null) {
            m_Selection = new List<LevelExit>();
        }

        m_Selection.Clear();

        LevelExit[] levelExits = BrushUtility.GetRootGrid().GetComponentsInChildren<LevelExit>();
        for (int i=0; i<levelExits.Length; ++i) {
            if (position.Contains(grid.WorldToCell(levelExits[i].transform.position))) {
                m_Selection.Add(levelExits[i]);
            }
        }
    }

    public override void Move(GridLayout grid, GameObject layer, BoundsInt from, BoundsInt to) {
        for (int i=0; i<m_Selection.Count; ++i) {
            m_Selection[i].transform.Translate(grid.CellToWorld(to.min) - grid.CellToWorld(from.min));
        }
    }
}
