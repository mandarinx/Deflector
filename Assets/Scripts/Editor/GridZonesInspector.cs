using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridZones))]
public class GridZonesInspector : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Recalculate")) {
            (target as GridZones)?.Recalculate();
        }
    }

    private void OnSceneGUI() {
        serializedObject.Update();

        SerializedProperty zones = serializedObject.FindProperty("zones");
        for (int i = 0; i < zones.arraySize; ++i) {
            SerializedProperty zone = zones.GetArrayElementAtIndex(i);
            Vector2 pivot = new Vector2(
                zone.FindPropertyRelative("x").floatValue, 
                zone.FindPropertyRelative("y").floatValue);
            Vector2 size = new Vector2(
                zone.FindPropertyRelative("width").floatValue, 
                zone.FindPropertyRelative("height").floatValue);

            // Pivot
            Handles.color = Color.white;
            Vector3 newPivot = Handles.Slider2D(
                pivot, 
                Vector3.forward, 
                Vector3.right, 
                Vector3.up, 
                0.25f, 
                Handles.RectangleHandleCap, 
                1f);
        
            // Size
            Vector2 newSize = RR.ResizeRect(
                pivot,
                size,
                Color.white, 
                new Color(1f, 0f, 0f, 0.25f), 
                1f);

            zone.FindPropertyRelative("x").floatValue = newPivot.x;
            zone.FindPropertyRelative("y").floatValue = newPivot.y;
            zone.FindPropertyRelative("width").floatValue = newSize.x;
            zone.FindPropertyRelative("height").floatValue = newSize.y;
        }

        serializedObject.ApplyModifiedProperties();
    }
}


public static class RR {
    public static Vector2 ResizeRect(Vector3 pivot, Vector3 size, Color capCol, Color fillCol, float snap) {
        Vector3[] corners = {
            pivot,
            pivot + Vector3.up * size.y,
            pivot + Vector3.up * size.y + Vector3.right * size.x,
            pivot + Vector3.right * size.x
        };

        Handles.DrawSolidRectangleWithOutline(corners, fillCol, capCol);

        Handles.color = capCol;
        return Handles.Slider2D(pivot + size, Vector3.forward, Vector3.up, Vector3.right, 0.25f, Handles.RectangleHandleCap, snap) - pivot;
    }
}