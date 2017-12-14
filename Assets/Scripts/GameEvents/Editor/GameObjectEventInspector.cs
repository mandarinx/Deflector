using UnityEditor;
using UnityEngine;

namespace GameEvents {
    
    [CustomEditor(typeof(GameObjectEvent))]
    public class GameObjectEventInspector : Editor {
        
        private GameObject go;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            go = (GameObject)EditorGUILayout.ObjectField(go, typeof(GameObject), true);

            if (GUILayout.Button("Raise")) {
                (target as GameObjectEvent)?.Invoke(go);
            }
        }
    }
}
