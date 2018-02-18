using UnityEngine;

namespace LunchGame01 {
    public static class TransformUtils {

        public static string GetFullPath(GameObject go) {
            return GetFullPath(go.transform);
        }

        public static string GetFullPath(Transform transform) {
            return transform.parent == null
                ? transform.name
                : GetFullPath(transform.parent) + "/" + transform.name;
        }
    }
}
