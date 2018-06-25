using UnityEditor;
using UnityEngine;

namespace GameEvents {

    [CustomEditor(typeof(GameObjectEvent))]
    public class GameObjectEventInspector : Editor {

        private GameObject go;
        private EventBindings<GameObjectEventListener, GameObjectEvent> bindings;

        private void OnEnable() {
            bindings = new EventBindings<GameObjectEventListener, GameObjectEvent>(target as GameObjectEvent);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            go = (GameObject)EditorGUILayout.ObjectField(go, typeof(GameObject), true);

            if (GUILayout.Button("Invoke")) {
                (target as GameObjectEvent)?.Invoke(go);
            }

            bindings.OnInspectorGUI();
        }
    }
}
