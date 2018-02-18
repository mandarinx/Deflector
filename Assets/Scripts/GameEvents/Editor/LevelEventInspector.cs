using UnityEditor;
using UnityEngine;
using LunchGame01;

namespace GameEvents {
    [CustomEditor(typeof(LevelEvent))]
    public class LevelEventInspector : Editor {

        private Level payload;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;

            payload = (Level) EditorGUILayout.ObjectField(payload, typeof(Level), true);

            if (GUILayout.Button("Invoke")) {
                (target as LevelEvent)?.Invoke(payload);
            }
        }
    }
}
