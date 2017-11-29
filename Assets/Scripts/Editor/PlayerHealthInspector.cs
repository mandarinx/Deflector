using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthInspector : Editor {

    public override void OnInspectorGUI() {
        serializedObject.Update();
        DrawDefaultInspector();
        if (GUILayout.Button("Kill")) {
            (target as PlayerHealth)?.SetLives(0);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
