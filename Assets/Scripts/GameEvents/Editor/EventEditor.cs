using UnityEditor;
using UnityEngine;

namespace GameEvents {

    [CustomEditor(typeof(GameEvent))]
    public class EventEditor : Editor {
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Raise")) {
                (target as GameEvent)?.Invoke();
            }
        }
    }
}
