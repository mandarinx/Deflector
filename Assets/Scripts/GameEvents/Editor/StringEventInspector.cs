using UnityEditor;
using UnityEngine;

namespace GameEvents {
    
    [CustomEditor(typeof(StringEvent))]
    public class StringEventInspector : Editor {
        
        private string payload;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            payload = EditorGUILayout.TextField(payload);

            if (GUILayout.Button("Raise")) {
                (target as StringEvent)?.Invoke(payload);
            }
        }
    }
}
