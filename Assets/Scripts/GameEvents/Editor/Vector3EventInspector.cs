using UnityEditor;
using UnityEngine;

namespace GameEvents {
    
    [CustomEditor(typeof(Vector3Event))]
    public class Vector3EventInspector : Editor {
        
        private Vector3 payload;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            payload = EditorGUILayout.Vector3Field("Vector3", payload);

            if (GUILayout.Button("Raise")) {
                (target as Vector3Event)?.Invoke(payload);
            }
        }
    }
}
