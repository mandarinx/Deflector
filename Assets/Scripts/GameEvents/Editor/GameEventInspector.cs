using UnityEditor;
using UnityEngine;

namespace GameEvents {

    [CustomEditor(typeof(GameEvent))]
    public class GameEventInspector : Editor {

        private EventBindings<GameEventListener, GameEvent> bindings;

        private void OnEnable() {
            bindings = new EventBindings<GameEventListener, GameEvent>(target as GameEvent);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Invoke")) {
                (target as GameEvent)?.Invoke();
            }

            bindings.OnInspectorGUI();
        }
    }
}
