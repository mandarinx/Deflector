using UnityEditor;

namespace LunchGame01 {
    public class BrushBaseInspector : GridBrushEditorBase {

        public override void OnInspectorGUI() {
            BrushUtility.ShowCurrentScene();
            DrawDefaultInspector();
        }
    }
}
