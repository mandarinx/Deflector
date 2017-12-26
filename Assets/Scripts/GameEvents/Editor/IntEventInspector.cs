using UnityEditor;
using UnityEngine;

namespace GameEvents {
    [CustomEditor(typeof(IntEvent))]
    public class IntEventInspector : Editor {

        private int payload;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            payload = EditorGUILayout.IntField("Payload", payload);
            if (GUILayout.Button("Invoke")) {
                (target as IntEvent)?.Invoke(payload);
            }
        }
    }
}
