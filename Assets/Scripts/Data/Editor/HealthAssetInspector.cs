using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HealthAsset))]
public class HealthAssetInspector : Editor {

    private int lives = 0;
    private int maxLives = 0;

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        DrawDefaultInspector();
        EditorGUILayout.Space();

        HealthAsset health = target as HealthAsset;
        
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
