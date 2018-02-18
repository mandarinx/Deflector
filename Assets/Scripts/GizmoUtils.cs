using UnityEngine;

namespace LunchGame01 {
    public static class GizmoUtils {

        public static void DrawCircle(Vector3 position, float radius, Color color) {
            Gizmos.color = color;
            float theta = 0f;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos = position + new Vector3(x, y, 0f);
            Vector3 lastPos = pos;
            const float TWOPI = Mathf.PI * 2;
            const float inc = 0.4f;
            for (theta = inc; theta < TWOPI; theta += inc) {
                x = radius * Mathf.Cos(theta);
                y = radius * Mathf.Sin(theta);
                Vector3 newPos = position + new Vector3(x, y, 0f);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);
        }
    }
}
