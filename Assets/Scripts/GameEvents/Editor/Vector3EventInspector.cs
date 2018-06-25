using UnityEditor;
using UnityEngine;

namespace GameEvents {

    [CustomEditor(typeof(Vector3Event))]
    public class Vector3EventInspector : Editor {

        private Vector3 payload;
        private EventBindings<Vector3EventListener, Vector3Event> bindings;

        private void OnEnable() {
            bindings = new EventBindings<Vector3EventListener, Vector3Event>(target as Vector3Event);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            payload = EditorGUILayout.Vector3Field("Vector3", payload);

            if (GUILayout.Button("Raise")) {
                (target as Vector3Event)?.Invoke(payload);
            }

            bindings.OnInspectorGUI();
        }
    }
}
