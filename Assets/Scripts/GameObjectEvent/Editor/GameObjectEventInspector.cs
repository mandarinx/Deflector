// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace RoboRyanTron.Unite2017.Events
{
    [CustomEditor(typeof(GameEvent))]
    public class GameObjectEventInspector : Editor {
        private GameObject go;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            go = (GameObject)EditorGUILayout.ObjectField(go, typeof(GameObject), true);
            GameObjectEvent e = target as GameObjectEvent;
            if (GUILayout.Button("Raise"))
                e.Raise(go);
        }
    }
}