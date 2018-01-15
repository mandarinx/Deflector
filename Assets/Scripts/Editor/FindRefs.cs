using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class FindRefs {

    [MenuItem("CONTEXT/Component/Find references to this")]
    private static void FindReferences(MenuCommand data) {
        Component comp = data.context as Component;
        if (comp != null) {
            FindReferencesTo(comp);
        }
    }
 
    [MenuItem("Assets/Find references to this")]
    private static void FindReferencesToAsset(MenuCommand data) {
        Object selected = Selection.activeObject;
        if (selected == null) {
            return;
        }
        FindReferencesTo(selected);
    }
 
    private static void FindReferencesTo(Object to) {
        List<Object> referencedBy = new List<Object>();
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        
        for (int j = 0; j < allObjects.Length; ++j) {
            GameObject go = allObjects[j];
 
            if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance
                && PrefabUtility.GetPrefabParent(go) == to) {

                Debug.Log($"Referenced by {go.name}, {go.GetType()}", go);
                referencedBy.Add(go);
            }
 
            Component[] components = go.GetComponents<Component>();
            
            for (int i = 0; i < components.Length; ++i) {
                Component c = components[i];
                if (c == null) {
                    continue;
                }
 
                SerializedObject so = new SerializedObject(c);
                SerializedProperty sp = so.GetIterator();
 
                while (sp.NextVisible(true)) {
                    if (sp.propertyType != SerializedPropertyType.ObjectReference) {
                        continue;
                    }

                    if (sp.objectReferenceValue != to) {
                        continue;
                    }
                    
                    Debug.Log($"Referenced by {c.name}, {c.GetType()}", c);
                    referencedBy.Add(c.gameObject);
                }
            }
        }

        if (referencedBy.Count == 0) {
            Debug.Log("No references in scene");
        }
    }
}
