using UnityEditor;
using UnityEngine;
using Deflector;

namespace GameEvents {
    [CustomEditor(typeof(LevelEvent))]
    public class LevelEventInspector : Editor {

        private Level payload;
        private EventBindings<LevelEventListener, LevelEvent> bindings;

        private void OnEnable() {
            bindings = new EventBindings<LevelEventListener, LevelEvent>(target as LevelEvent);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;

            payload = (Level) EditorGUILayout.ObjectField(payload, typeof(Level), true);

            if (GUILayout.Button("Invoke")) {
                (target as LevelEvent)?.Invoke(payload);
            }

            bindings.OnInspectorGUI();
        }
    }
}
