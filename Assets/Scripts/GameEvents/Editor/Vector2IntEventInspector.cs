using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vector2IntEvent))]
public class Vector2IntEventInspector : Editor {

    private Vector2Int payload;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUI.enabled = Application.isPlaying;
        payload = EditorGUILayout.Vector2IntField("Payload", payload);
        if (GUILayout.Button("Invoke")) {
            (target as Vector2IntEvent)?.Invoke(payload);
        }
    }
}
