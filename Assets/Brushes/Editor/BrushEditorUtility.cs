using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
public class BrushEditorUtility 
{
	const string k_CameraName = "MainCamera";
	private static Material s_GizmoMaterial;

	private static void InitializeMaterial(Color color)
	{
		if (s_GizmoMaterial == null)
			s_GizmoMaterial = new Material(Shader.Find("Unlit/GizmoShader"));

		s_GizmoMaterial.color = color;
		s_GizmoMaterial.SetPass(0);
	}

	public static void DrawQuad(GridLayout grid, Vector3Int position, Color color)
	{
		BeginQuads(color);
		DrawQuadBatched(grid, position);
		EndQuads();
	}

	public static void DrawMarquee(GridLayout grid, Vector3Int position, Color color)
	{
		BeginMarquee(color);
		DrawMarqueeBatched(grid, position);
		EndMarquee();
	}

	public static void DrawLine(GridLayout grid, Vector3Int from, Vector3Int to, Color color)
	{
		BeginLines(color);
		DrawLineBatched(grid, from, to);
		EndLines();
	}
	public static void BeginLines(Color color)
	{
		InitializeMaterial(color);
		GL.PushMatrix();
		GL.Begin(GL.LINES);
	}

	public static void BeginMarquee(Color color)
	{
		BeginLines(color);
	}

	public static void BeginQuads(Color color)
	{
		InitializeMaterial(color);
		GL.PushMatrix();
		GL.Begin(GL.QUADS);
	}

	public static void EndQuads()
	{
		GL.End();
		GL.PopMatrix();		
	}

	public static void EndLines()
	{
		GL.End();
		GL.PopMatrix();
	}

	public static void EndMarquee()
	{
		EndLines();
	}


	public static void DrawLineBatched(GridLayout grid, Vector3Int from, Vector3Int to)
	{
		GL.Vertex(grid.GetComponent<Grid>().GetCellCenterWorld(from));
		GL.Vertex(grid.GetComponent<Grid>().GetCellCenterWorld(to));
	}

	public static void DrawSplitLineBatched(GridLayout grid, Vector3Int from, Vector3Int direction)
	{
		if (direction == Vector3Int.up)
		{
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 1, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 1, 0)));
		}
		else if (direction == Vector3Int.right)
		{
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 1, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 0, 0)));
		}
		else if (direction == Vector3Int.down)
		{
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 0, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(1, 0, 0)));
		}
		else if (direction == Vector3Int.left)
		{
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 1, 0)));
			GL.Vertex(grid.CellToWorld(from + new Vector3Int(0, 0, 0)));
		}
	}

	public static void DrawMarqueeBatched(GridLayout grid, Vector3Int position)
	{
		GL.Vertex(grid.CellToWorld(position));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up));

		GL.Vertex(grid.CellToWorld(position + Vector3Int.up));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up + Vector3Int.right));

		GL.Vertex(grid.CellToWorld(position + Vector3Int.up + Vector3Int.right));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.right));

		GL.Vertex(grid.CellToWorld(position + Vector3Int.right));
		GL.Vertex(grid.CellToWorld(position));
	}

	public static void DrawQuadBatched(GridLayout grid, Vector3Int position)
	{
		GL.Vertex(grid.CellToWorld(position));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.up + Vector3Int.right));
		GL.Vertex(grid.CellToWorld(position + Vector3Int.right));
	}

	public static void UnpreparedSceneInspector()
	{
		GUILayout.Space(5f);
		GUILayout.Label("This scene is not yet ready for level editing.");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Initialize Scene"))
		{
			PrepareScene();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	public static bool SceneIsPrepared() {
		bool prepared = GameObject.Find(k_CameraName) != null;
		prepared &= BrushUtility.GetRootGrid(false);
		return prepared;
	}
	
	public static void PrepareScene()
	{
		GameObject cam = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MainCamera.prefab");
		GameObject sfx = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SoundEffects.prefab");
		GameObject score = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ScoreCounter.prefab");
		GameObject ui = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI.prefab");
		GameObject turrets = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Turrets.prefab");
		GameObject players = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players.prefab");

		if (cam != null && 
			sfx != null && 
			score != null && 
			ui != null && 
			players != null && 
			turrets != null) {
			
			RenderSettings.ambientLight = Color.white;
			foreach (Camera c in Object.FindObjectsOfType<Camera>()) {
				Object.DestroyImmediate(c.gameObject, false);
			}
			
			PrefabUtility.InstantiatePrefab(cam);
			PrefabUtility.InstantiatePrefab(sfx);
			PrefabUtility.InstantiatePrefab(score);
			PrefabUtility.InstantiatePrefab(ui);
			PrefabUtility.InstantiatePrefab(turrets);
			PrefabUtility.InstantiatePrefab(players);

//			GameObject wallsGo = PrefabUtility.InstantiatePrefab(sfx) as GameObject;
//			PrefabUtility.DisconnectPrefabInstance(wallsGo);

//			LevelBrush.ResetLevelCache();
//			TintTextureGenerator.RefreshTintmap();
		} else {
			Debug.LogWarning("Some prefabs for initializing the scene are missing.");
		}
	}

	public static void AutoSelectGrid()
	{
		if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponentInParent<Grid>() == null)
		{
			Grid grid = BrushUtility.GetRootGrid(false);
			if(grid)
				Selection.activeTransform = grid.transform;
		}
	}

	public static void AutoSelectLayer(string name)
	{
		Transform transform = Selection.activeTransform;
		if (transform != null)
		{
			while (transform.parent != null)
			{
				if (transform.name == name)
				{
					return;
				}
				transform = transform.parent;
			}
		}

		AutoSelectGrid();
	}
}
#endif
