using UnityEditor;
using UnityEngine;

namespace GameEvents {
    [CustomEditor(typeof(Vector3IntEvent))]
    public class Vector3IntEventInspector : Editor {

        private Vector3Int payload;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            payload = EditorGUILayout.Vector3IntField("Payload", payload);
            if (GUILayout.Button("Invoke")) {
                (target as Vector3IntEvent)?.Invoke(payload);
            }
        }
    }
}
