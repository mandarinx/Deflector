using UnityEditor;
using UnityEngine;

namespace GameEvents {

    [CustomEditor(typeof(StringEvent))]
    public class StringEventInspector : Editor {

        private string payload;
        private EventBindings<StringEventListener, StringEvent> bindings;

        private void OnEnable() {
            bindings = new EventBindings<StringEventListener, StringEvent>(target as StringEvent);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            payload = EditorGUILayout.TextField(payload);

            if (GUILayout.Button("Raise")) {
                (target as StringEvent)?.Invoke(payload);
            }

            bindings.OnInspectorGUI();
        }
    }
}
