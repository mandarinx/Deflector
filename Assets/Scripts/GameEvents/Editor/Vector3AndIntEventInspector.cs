using UnityEditor;
using UnityEngine;

namespace GameEvents {
    [CustomEditor(typeof(Vector3AndIntEvent))]
    public class Vector3AndIntEventInspector : Editor {

        private Vector3 payload1;
        private int payload2;
        private EventBindings<Vector3AndIntEventListener, Vector3AndIntEvent> bindings;

        private void OnEnable() {
            bindings = new EventBindings<Vector3AndIntEventListener, Vector3AndIntEvent>(target as Vector3AndIntEvent);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            payload1 = EditorGUILayout.Vector3Field("Payload1", payload1);
            payload2 = EditorGUILayout.IntField("Payload2", payload2);
            if (GUILayout.Button("Invoke")) {
                (target as Vector3AndIntEvent)?.Invoke(payload1, payload2);
            }
            bindings.OnInspectorGUI();
        }
    }
}
