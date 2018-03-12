using UnityEditor;

namespace Deflector {
    public class BrushBaseInspector : GridBrushEditorBase {

        public override void OnInspectorGUI() {
            BrushUtility.ShowCurrentScene();
            DrawDefaultInspector();
        }
    }
}
