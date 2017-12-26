using UnityEditor;
using UnityEngine;

namespace GameEvents {

    [CustomEditor(typeof(GameEvent))]
    public class GameEventInspector : Editor {
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Invoke")) {
                (target as GameEvent)?.Invoke();
            }
        }
    }
}
