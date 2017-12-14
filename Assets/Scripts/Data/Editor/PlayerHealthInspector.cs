using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthInspector : Editor {

    private int lives = 0;
    private int maxLives = 0;

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        DrawDefaultInspector();
        EditorGUILayout.Space();

        PlayerHealth health = target as PlayerHealth;
        
        lives = EditorGUILayout.IntField("Lives", lives);
        if (GUILayout.Button("Set Lives")) {
            health?.SetLives(lives);
        }
        
        EditorGUILayout.Space();
        
        maxLives = EditorGUILayout.IntField("Max Lives", maxLives);
        if (GUILayout.Button("Set Max Lives")) {
            health?.SetMaxLives(maxLives);
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Kill")) {
            health?.SetLives(0);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
