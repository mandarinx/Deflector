using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CreateGameEvent : ScriptableWizard {

    public string eventNamespace;
    public string eventClassType;
    public string payloadClassType;

    private static readonly Dictionary<string, Func<string, string>> inspectorFields = new Dictionary<string, Func<string, string>> {
        { "Object", c => $"payload = ({c})EditorGUILayout.ObjectField(payload, typeof({c}), false);" },
        { "Vector2Int", c => "payload = EditorGUILayout.Vector2IntField(\"Payload\", payload);" },
        { "Vector2", c => "payload = EditorGUILayout.Vector2Field(\"Payload\", payload);" },
        { "Vector3Int", c => "payload = EditorGUILayout.Vector3IntField(\"Payload\", payload);" },
        { "Vector3", c => "payload = EditorGUILayout.Vector3Field(\"Payload\", payload);" },
        { "Vector4", c => "payload = EditorGUILayout.Vector4Field(\"Payload\", payload);" },
        { "int", c => "payload = EditorGUILayout.IntField(\"Payload\", payload);" },
        { "long", c => "payload = EditorGUILayout.LongField(\"Payload\", payload);" },
        { "float", c => "payload = EditorGUILayout.FloatField(\"Payload\", payload);" },
        { "double", c => "payload = EditorGUILayout.DoubleField(\"Payload\", payload);" },
        { "Color", c => "payload = EditorGUILayout.ColorField(\"Payload\", payload);" },
        { "Color32", c => "payload = EditorGUILayout.ColorField(\"Payload\", payload);" },
        { "Rect", c => "payload = EditorGUILayout.RectField(\"Payload\", payload);" },
        { "RectInt", c => "payload = EditorGUILayout.RectIntField(\"Payload\", payload);" },
        { "AnimationCurve", c => "payload = EditorGUILayout.CurveField(\"Payload\", payload);" },
        { "Bounds", c => "payload = EditorGUILayout.BoundsField(\"Payload\", payload);" },
        { "BoundsInt", c => "payload = EditorGUILayout.BoundsIntField(\"Payload\", payload);" },
    };
    // Enum
    // IntSlider
    // Layer
    // Mask
    // Tag
    // Text
    // Toggle (bool)

    [MenuItem("Tools/Hyper Games/Create Game Event")]
    public static void Create() {
        DisplayWizard<CreateGameEvent>("Create a Game Event", "Create");
    }

    private void OnWizardCreate() {
        if (string.IsNullOrEmpty(eventClassType)) {
            Debug.LogError("Cannot create event class with empty name.");
            return;
        }

        string eventClassName = $"{eventClassType}Event";
        string eventListenerClassName = $"{eventClassName}Listener";
        string eventInspectorClassName = $"{eventClassName}Inspector";

        ClassPrinter gameEvent = new ClassPrinter()
            .Lines(
                "using System.Collections.Generic;",
                "using UnityEngine;")
            .EmptyLine();

        if (!string.IsNullOrEmpty(eventNamespace)) {
            gameEvent
                .Line($"namespace {eventNamespace} {{")
                .EmptyLine()
                .Indent();
        }

        gameEvent
            .Lines(
                $"[CreateAssetMenu(menuName = \"Game Events/{eventClassType}\", fileName = \"{eventClassName}.asset\")]",
                $"public class {eventClassName} : ScriptableObject {{")
            .Indent()
                .Line($"private readonly List<{eventListenerClassName}> eventListeners = new List<{eventListenerClassName}>();")
                .EmptyLine()

                .Line($"public void Invoke({payloadClassType} payload) {{")
                .Indent()
                    .Line("for (int i = eventListeners.Count -1; i >= 0; i--) {")
                    .Indent()
                        .Line("eventListeners[i].OnEventInvoked(payload);")
                    .Outdent()
                    .Line("}")
                .Outdent()
                .Line("}")

                .EmptyLine()

                .Line($"public void AddListener({eventListenerClassName} listener) {{")
                .Indent()
                    .Line("if (!eventListeners.Contains(listener)) {")
                    .Indent()
                        .Line("eventListeners.Add(listener);")
                    .Outdent()
                    .Line("}")
                .Outdent()
                .Line("}")

                .EmptyLine()

                .Line($"public void RemoveListener({eventListenerClassName} listener) {{")
                .Indent()
                    .Line("eventListeners.Remove(listener);")
                .Outdent()
                .Line("}")

            .Outdent()
            .Line("}");

        if (!string.IsNullOrEmpty(eventNamespace)) {
            gameEvent
                .Outdent()
                .Line("}");
        }

        ClassPrinter gameEventListener = new ClassPrinter()
            .Lines(
                "using System;",
                "using UnityEngine;",
                "using UnityEngine.Events;")
            .EmptyLine();

        if (!string.IsNullOrEmpty(eventNamespace)) {
            gameEventListener
                .Line($"namespace {eventNamespace} {{")
                .EmptyLine()
                .Indent();
        }

        gameEventListener
            .Lines(
                "[Serializable]",
                $"public class Unity{eventClassName} : UnityEvent<{payloadClassType}> {{}}")
            .EmptyLine()

            .Lines(
                $"[AddComponentMenu(\"Game Events/{eventListenerClassName}\")]",
                $"public class {eventListenerClassName} : MonoBehaviour {{")
            .EmptyLine()
            .Indent()

                .Lines(
                    $"public {eventClassName} evt;",
                    $"public Unity{eventClassName} response;")
                .EmptyLine()

                .Line("private void OnEnable() {")
                .Indent()
                    .Line("evt.AddListener(this);")
                .Outdent()
                .Line("}")
                .EmptyLine()

                .Line("private void OnDisable() {")
                .Indent()
                    .Line("evt.RemoveListener(this);")
                .Outdent()
                .Line("}")
                .EmptyLine()

                .Line($"public void OnEventInvoked({payloadClassType} payload) {{")
                .Indent()
                    .Line("response.Invoke(payload);")
                .Outdent()
                .Line("}")

            .Outdent()
            .Line("}");

        if (!string.IsNullOrEmpty(eventNamespace)) {
            gameEventListener
                .Outdent()
                .Line("}");
        }

        ClassPrinter inspector = new ClassPrinter()
            .Lines(
                "using UnityEditor;",
                "using UnityEngine;")
            .EmptyLine();

        if (!string.IsNullOrEmpty(eventNamespace)) {
            inspector
                .Line($"namespace {eventNamespace} {{")
                .Indent();
        }

        inspector
            .Lines(
                $"[CustomEditor(typeof({eventClassName}))]",
                $"public class {eventInspectorClassName} : Editor {{")
            .EmptyLine()
            .Indent()

                .Lines(
                    $"private {payloadClassType} payload;",
                    $"private EventBindings<{eventListenerClassName}, {eventClassName}> bindings;")
                .EmptyLine()

                .Line("private void OnEnable() {")
                    .Indent()
                    .Line($"bindings = new EventBindings<{eventListenerClassName}, {eventClassName}>(target as {eventClassName});")
                    .Outdent()
                .Line("}")

                .Line("public override void OnInspectorGUI() {")
                    .Indent()

                    .Lines(
                        "base.OnInspectorGUI();",
                        "GUI.enabled = Application.isPlaying;",
                        GetInspectorField(payloadClassType),
                        "if (GUILayout.Button(\"Invoke\")) {")
                        .Indent()

                            .Line($"(target as {eventClassName})?.Invoke(payload);")

                        .Outdent()
                        .Line("}")

                    .Line("bindings.OnInspectorGUI();")
                    .Outdent()
                .Line("}")

            .Outdent()
            .Line("}");

        if (!string.IsNullOrEmpty(eventNamespace)) {
            inspector
                .Outdent()
                .Line("}");
        }

        if (!Directory.Exists(GetSelectedFolderFull() + "/Editor")) {
            AssetDatabase.CreateFolder(GetSelectedFolderRel(), "Editor");
        }

        File.WriteAllText(GetSelectedFolderRel() + $"/{eventClassName}.cs", gameEvent.ToString());
        File.WriteAllText(GetSelectedFolderRel() + $"/{eventListenerClassName}.cs", gameEventListener.ToString());
        File.WriteAllText(GetSelectedFolderRel() + $"/Editor/{eventInspectorClassName}.cs", inspector.ToString());

        AssetDatabase.Refresh();
    }

    private void OnWizardUpdate() {
        helpString = "Select destination folder in Project view. Editor scripts will be placed in an Editor sub folder. "+
        "Currently selected folder is " + GetSelectedFolderRel();
    }

    private static string GetSelectedFolderRel() {
        string path = "Assets";
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
            path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(path)) {
                path = Path.GetDirectoryName(path);
            }
            break;
        }
        return path;
    }

    private static string GetSelectedFolderFull() {
        string path = Application.dataPath.Replace("Assets", "");
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
            path += AssetDatabase.GetAssetPath(obj);
            if (File.Exists(path)) {
                path = Path.GetDirectoryName(path);
            }
            break;
        }
        return path;
    }

    private static string GetInspectorField(string classType) {
        Func<string, string> field = str => "";
        return inspectorFields.TryGetValue(classType, out field) ? field(classType) : "//NOT IMPLEMENTED";
    }
}

public class ClassPrinter {

    private const string tab = "    ";
    private int indent = 0;

    private string content = "";

    public ClassPrinter Indent() {
        ++indent;
        return this;
    }

    public ClassPrinter Outdent() {
        indent = Mathf.Max(indent - 1, 0);
        return this;
    }

    public ClassPrinter Line(string line) {
        content += $"{GetIndent()}{line}\n";
        return this;
    }

    public ClassPrinter Lines(params string[] lines) {
        for (int i = 0; i < lines.Length; ++i) {
            content += $"{GetIndent()}{lines[i]}\n";
        }
        return this;
    }

    public ClassPrinter EmptyLine() {
        content += "\n";
        return this;
    }

    private string GetIndent() {
        string totalIndent = "";
        for (int i = 0; i < indent; ++i) {
            totalIndent += tab;
        }
        return totalIndent;
    }

    public override string ToString() {
        return content;
    }
}
