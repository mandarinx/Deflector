using UnityEditor;
using UnityEngine;
using Deflector;

namespace GameEvents {
    [CustomEditor(typeof(LayersEvent))]
    public class LayersEventInspector : Editor {

        private Layers payload;
        private EventBindings<LayersEventListener, LayersEvent> bindings;

        private void OnEnable() {
            bindings = new EventBindings<LayersEventListener, LayersEvent>(target as LayersEvent);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;

            payload = (Layers) EditorGUILayout.ObjectField("Layers", payload, typeof(Level), false);

            if (GUILayout.Button("Invoke")) {
                (target as LayersEvent)?.Invoke(payload);
            }

            bindings.OnInspectorGUI();
        }
    }
}
