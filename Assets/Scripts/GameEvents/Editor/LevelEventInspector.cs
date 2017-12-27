using UnityEditor;
using UnityEngine;

namespace GameEvents {
    [CustomEditor(typeof(LevelEvent))]
    public class LevelEventInspector : Editor {

        private Level payload;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;

            payload = (Level) EditorGUILayout.ObjectField("Level", payload, typeof(Level), false);
            
            if (GUILayout.Button("Invoke")) {
                (target as LevelEvent)?.Invoke(payload);
            }
        }
    }
}
