using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Level))]
public class LevelInspector : Editor {

    private ReorderableList gameModes;

    private void OnEnable() {
        gameModes = new ReorderableList(
            serializedObject:    serializedObject,
            elements:            serializedObject.FindProperty("gameModes"),
            draggable:           false,
            displayAddButton:    true,
            displayHeader:       true,
            displayRemoveButton: true) {
            
            drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight), 
                    gameModes.serializedProperty.GetArrayElementAtIndex(index),
                    new GUIContent($"Game Mode {index:00}"));
            },

            elementHeightCallback = index => EditorGUIUtility.singleLineHeight + 6,

            drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Game Modes"); }
        };
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        SceneField(serializedObject.FindProperty("scenePath"), "Scene");

        gameModes.DoLayoutList();
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
