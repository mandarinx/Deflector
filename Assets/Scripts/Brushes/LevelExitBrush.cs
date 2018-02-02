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
    private string 		    layerName;
    [SerializeField]
    private Tile            closedDoor;
    [SerializeField]
    private GameObject      prefab;
    [SerializeField]
    private Vector3 	    prefabOffset;

    private List<LevelExit> selection;

    public string           Layer => layerName;

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
            prefab,
            BrushUtility.GetWorldPos(grid, position + prefabOffset),
            BrushUtility.GetLayer(layerName));

        Tilemap tm = BrushUtility
           .GetLayer(layerName)
           .GetComponent<Tilemap>();

        if (tm == null) {
            return;
        }

        le.SetGridAndTilemap(grid, tm);

        BrushUtility.RegisterUndo(tm, $"Add LevelExit tile to Grid");
        tm.SetTile(grid.WorldToCell(le.transform.position), closedDoor);
    }

    public override void Erase(GridLayout grid, GameObject layer, Vector3Int position) {
        LevelExit[] exits = layer.GetComponentsInChildren<LevelExit>();
        for (int i = 0; i < exits.Length; ++i) {
            if (position != grid.WorldToCell(exits[i].transform.position)) {
                continue;
            }
            layer
               .GetComponent<Tilemap>()
              ?.SetTile(grid.WorldToCell(exits[i].transform.position), null);
            BrushUtility.Destroy(exits[i].gameObject);
        }
	}

    public override void Select(GridLayout grid, GameObject layer, BoundsInt position) {
        base.Select(grid, layer, position);

        if (selection == null) {
            selection = new List<LevelExit>();
        }

        selection.Clear();

        LevelExit[] levelExits = BrushUtility.GetRootGrid().GetComponentsInChildren<LevelExit>();
        for (int i=0; i<levelExits.Length; ++i) {
            if (position.Contains(grid.WorldToCell(levelExits[i].transform.position))) {
                selection.Add(levelExits[i]);
            }
        }
    }

    public override void Move(GridLayout grid, GameObject layer, BoundsInt from, BoundsInt to) {
        for (int i=0; i<selection.Count; ++i) {
            selection[i].transform.Translate(grid.CellToWorld(to.min) - grid.CellToWorld(from.min));
        }
    }
}
