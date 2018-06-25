using UnityEngine;
using UnityEditor;
using Deflector;

[CustomEditor(typeof(Level))]
public class LevelInspector : Editor {

    public override void OnInspectorGUI() {
        serializedObject.Update();
        SceneField(serializedObject.FindProperty("scenePath"), "Scene");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameMode"));
        serializedObject.ApplyModifiedProperties();
    }

    private static void SceneField(SerializedProperty pathProperty, string label) {
        SceneAsset sceneAsset = null;

        if (!string.IsNullOrEmpty(pathProperty.stringValue)) {
            sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathProperty.stringValue + ".unity");
        }

        sceneAsset = (SceneAsset) EditorGUILayout.ObjectField(label, sceneAsset, typeof(SceneAsset), false);
        pathProperty.stringValue = AssetDatabase.GetAssetPath(sceneAsset).Replace(".unity", "");
    }

    private static void SceneField(Rect rect, SerializedProperty pathProperty, string label) {
        SceneAsset sceneAsset = null;

        if (!string.IsNullOrEmpty(pathProperty.stringValue)) {
            sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathProperty.stringValue + ".unity");
        }

        sceneAsset = (SceneAsset) EditorGUI.ObjectField(rect, label, sceneAsset, typeof(SceneAsset), false);
        pathProperty.stringValue = AssetDatabase.GetAssetPath(sceneAsset).Replace(".unity", "");
    }
}
