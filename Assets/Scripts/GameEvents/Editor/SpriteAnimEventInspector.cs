using PowerTools;
using UnityEditor;
using UnityEngine;

namespace GameEvents {
    
    [CustomEditor(typeof(SpriteAnimEvent))]
    public class SpriteAnimEventInspector : Editor {
        
        private SpriteAnim payload;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            payload = (SpriteAnim)EditorGUILayout.ObjectField(payload, typeof(SpriteAnim), true);

            if (GUILayout.Button("Invoke")) {
                (target as SpriteAnimEvent)?.Invoke(payload);
            }
        }
    }
}
