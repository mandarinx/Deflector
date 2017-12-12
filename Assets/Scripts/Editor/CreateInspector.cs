using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public static class CreateInspector {

    [MenuItem("Assets/Create/C# Inspector", false, 82)]
    public static void CreateInspectorForSelectedObject() {
        UnityEngine.Object obj = Selection.activeObject;

        string dataPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        string assetPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
        assetPath = assetPath.Substring(6);
        string fullPath = dataPath + "Assets" + assetPath;

        string fileName = Path.GetFileName(fullPath);
        string assetDir = assetPath.Replace(fileName, "");

        if (!Directory.Exists(dataPath + "Assets" + assetDir + "Editor")) {
            AssetDatabase.CreateFolder("Assets" + assetDir.Substring(0,assetDir.Length-1), "Editor");
        }

        string className = "";
        string ns = "";
        string inspectorName = "";
        string[] lines = File.ReadAllLines(fullPath);
        foreach (string line in lines) {
            if (line.Contains("namespace ")) {
                ns = line.Replace("namespace", "");
                ns = ns.Replace("{", "");
                ns = ns.Replace(" ", "");
            }

            if (line.Contains("public ") &&
                line.Contains("class ") &&
                (line.Contains("MonoBehaviour") ||
                 line.Contains("ScriptableObject"))) {
                className = line.Replace("public", "");
                className = className.Replace("class", "");
                int colon = className.IndexOf(":", StringComparison.CurrentCulture);
                className = className.Substring(0, colon);
                className = className.Replace(" ", "");
                inspectorName = className + "Inspector";
            }
        }

        if (className == "") {
            Debug.LogWarning("Cannot find classname in "+fileName);
            return;
        }

        string indent = "";

        List<string> contents = new List<string>();
        contents.Add("using UnityEngine;");
        contents.Add("using UnityEditor;");
        contents.Add("");
        if (ns != "") {
            indent = "    ";
            contents.Add("namespace "+ns+" {");
            contents.Add("");
        }
        contents.Add(indent + "[CustomEditor(typeof("+className+"))]");
        contents.Add(indent + "public class "+inspectorName+" : Editor {");
        contents.Add("");
        contents.Add(indent + "    private void OnEnable() {");
        contents.Add(indent + "    }");
        contents.Add("");
        contents.Add(indent + "    public override void OnInspectorGUI() {");
        contents.Add(indent + "        serializedObject.Update();");
        contents.Add(indent + "        serializedObject.ApplyModifiedProperties();");
        contents.Add(indent + "    }");
        contents.Add("");
        contents.Add(indent + "    private void OnSceneGUI() {");
        contents.Add(indent + "    }");
        contents.Add(indent + "}");

        if (ns != "") {
            contents.Add("}");
        }

        string inspectorPath = AssetDatabase.GenerateUniqueAssetPath("Assets" + assetDir + "Editor/" + inspectorName + ".cs");

        File.WriteAllLines(inspectorPath, contents.ToArray());
        AssetDatabase.Refresh();

        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(inspectorPath);
    }

    [MenuItem("Assets/Create/C# Inspector", true, 82)]
    static public bool CreateInspectorValidator() {
        return Selection.activeObject is MonoScript;
    }
}
