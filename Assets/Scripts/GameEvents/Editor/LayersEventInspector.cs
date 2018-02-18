using UnityEditor;
using UnityEngine;
using LunchGame01;

namespace GameEvents {
    [CustomEditor(typeof(LayersEvent))]
    public class LayersEventInspector : Editor {

        private Layers payload;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;

            payload = (Layers) EditorGUILayout.ObjectField("Layers", payload, typeof(Level), false);

            if (GUILayout.Button("Invoke")) {
                (target as LayersEvent)?.Invoke(payload);
            }
        }
    }
}
