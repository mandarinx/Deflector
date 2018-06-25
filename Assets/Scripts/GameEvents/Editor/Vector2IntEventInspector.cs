using GameEvents;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vector2IntEvent))]
public class Vector2IntEventInspector : Editor {

    private Vector2Int payload;
    private EventBindings<Vector2IntEventListener, Vector2IntEvent> bindings;

    private void OnEnable() {
        bindings = new EventBindings<Vector2IntEventListener, Vector2IntEvent>(target as Vector2IntEvent);
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUI.enabled = Application.isPlaying;
        payload = EditorGUILayout.Vector2IntField("Payload", payload);
        if (GUILayout.Button("Invoke")) {
            (target as Vector2IntEvent)?.Invoke(payload);
        }
        bindings.OnInspectorGUI();
    }
}
