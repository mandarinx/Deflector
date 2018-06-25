using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Deflector {

    [CustomEditor(typeof(UIPanel))]
    public class UIPanelInspector : Editor {

        private ReorderableList    optionRefs;
        private SerializedProperty panelRefs;

        private void OnEnable() {
            panelRefs = serializedObject.FindProperty("panelRefs");
            optionRefs = new ReorderableList(serializedObject:    serializedObject,
                                             elements:            serializedObject.FindProperty("menuOptions"),
                                             draggable:           false,
                                             displayHeader:       true,
                                             displayAddButton:    true,
                                             displayRemoveButton: true) {

                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Menu Option Panel Refs"); },

                drawElementCallback = (rect, index, active, focused) => {
                    float line = EditorGUIUtility.singleLineHeight;
                    SerializedProperty menuOpt = optionRefs.serializedProperty.GetArrayElementAtIndex(index);
                    SerializedProperty panelRef = panelRefs.GetArrayElementAtIndex(index);
                    menuOpt.objectReferenceValue = EditorGUI.ObjectField(
                        new Rect(rect.x, rect.y, rect.width / 2, line),
                        menuOpt.objectReferenceValue,
                        typeof(MenuOption),
                        true);
                    panelRef.objectReferenceValue = EditorGUI.ObjectField(
                        new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, line),
                        panelRef.objectReferenceValue,
                        typeof(UIPanelRef),
                        false);
                },

                onAddCallback = rlist => {
                    int indexMenuOpts = rlist.serializedProperty.arraySize;
                    rlist.serializedProperty.arraySize++;
                    rlist.index = indexMenuOpts;

                    panelRefs.arraySize++;
                },

                onRemoveCallback = rlist => {
                    ReorderableList.defaultBehaviours.DoRemoveButton(rlist);
                },

                elementHeight = EditorGUIUtility.singleLineHeight + 4
            };
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("panelRef"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onEnterPanel"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onClosePanel"));
            optionRefs.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
