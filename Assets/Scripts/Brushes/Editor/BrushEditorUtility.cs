using UnityEngine;

public static class BrushEditorUtility {

	private static Material s_GizmoMaterial;

	private static void InitializeMaterial(Color color) {
	    if (s_GizmoMaterial == null) {
	        s_GizmoMaterial = new Material(Shader.Find("Unlit/Tilemap Gizmo"));
	    }

		s_GizmoMaterial.color = color;
		s_GizmoMaterial.SetPass(0);
	}

	public static void DrawQuad(GridLayout grid, Vector3Int position, Color color) {
		BeginQuads(color);
		DrawQuadBatched(grid, position);
		EndQuads();
	}

	public static void DrawMarquee(GridLayout grid, Vector3Int position, Color color) {
		BeginMarquee(color);
		DrawMarqueeBatched(grid, position);
		EndMarquee();
	}

	public static void DrawLine(GridLayout grid, Vector3Int from, Vector3Int to, Color color) {
		BeginLines(color);
		DrawLineBatched(grid, from, to);
		EndLines();
	}

	public static void BeginLines(Color color) {
		InitializeMaterial(color);
		GL.PushMatrix();
		GL.Begin(GL.LINES);
	}

	public static void BeginMarquee(Color color) {
		BeginLines(color);
	}

	public static void BeginQuads(Color color) {
		InitializeMaterial(color);
		GL.PushMatrix();
		GL.Begin(GL.QUADS);
	}

	public static void EndQuads() {
		GL.End();
		GL.PopMatrix();
	}

	public static void EndLines() {
		GL.End();
		GL.PopMatrix();
	}

	public static void EndMarquee() {
		EndLines();
	}

	public static void DrawLineBatched(GridLayout grid, Vector3Int from, Vector3Int to) {
		GL.Vertex(grid.GetComponent<Grid>().GetCellCenterWorld(from));
		GL.Vertex(grid.GetComponent<Grid>().GetCellCenterWorld(to));
	}

	public static void DrawSplitLineBatched(GridLayout grid, Vector3Int from, Vector3Int direction) {
		if (direction == Vector3Int.up) {
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 1, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 1, 0)));
		} else if (direction == Vector3Int.right) {
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 1, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 0, 0)));
		} else if (direction == Vector3Int.down) {
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 0, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 0, 0)));
		} else if (direction == Vector3Int.left) {
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 1, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 0, 0)));
		}
	}

	public static void DrawMarqueeBatched(GridLayout grid, Vector3Int position) {
		GL.Vertex(grid.CellToWorld(position));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up));

		GL.Vertex(grid.CellToWorld(position + Vector3Int.up));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up + Vector3Int.right));

		GL.Vertex(grid.CellToWorld(position + Vector3Int.up + Vector3Int.right));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.right));

		GL.Vertex(grid.CellToWorld(position + Vector3Int.right));
		GL.Vertex(grid.CellToWorld(position));
	}

	public static void DrawQuadBatched(GridLayout grid, Vector3Int position) {
		GL.Vertex(grid.CellToWorld(position));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up + Vector3Int.right));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.right));
	}
}
