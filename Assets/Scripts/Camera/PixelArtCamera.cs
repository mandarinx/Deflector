using UnityEngine;

namespace LunchGame01 {

    public class PixelArtCamera : MonoBehaviour {

        [SerializeField]
        private Vector2Int    targetRes;
        [SerializeField]
        private Material      rtMaterial;

        private RenderTexture rt;
        private Camera        cam;

        public Vector2Int     TargetResolution => targetRes;

        private void Awake() {
            cam = GetComponent<Camera>();
            rt = new RenderTexture(targetRes.x,
                                   targetRes.y,
                                   0,
                                   RenderTextureFormat.ARGB32)
            {
                filterMode       = FilterMode.Point,
                autoGenerateMips = false,
                anisoLevel       = 0,
                wrapMode         = TextureWrapMode.Clamp
            };
            cam.targetTexture = rt;
        }

        private void OnPreRender() {
            cam.targetTexture = rt;
        }

        private void OnPostRender() {
            cam.targetTexture = null;

            int scale   = BestFitScale(targetRes);
            int width   = targetRes.x * scale;
            int height  = targetRes.y * scale;

            // Explicitly set the x, y, z, w components to make it
            // easier to read the code in ScreenSpaceTexture.shader.
            Vector4 pos = new Vector4();
            pos.x = Mathf.FloorToInt((Screen.width - width) * 0.5f);
            pos.y = Mathf.FloorToInt((Screen.height - height) * 0.5f);
            pos.z = pos.x + width;
            pos.w = pos.y + height;

            rtMaterial.SetVector("_Pos", pos);
            rtMaterial.SetTexture("_MainTex", rt);
            rtMaterial.SetColor("_BackgroundColor", cam.backgroundColor);

            Graphics.Blit(rt, null, rtMaterial);
        }

        public static int BestFitScale(Vector2Int res) {
            return Mathf.Min(
                Mathf.FloorToInt(Mathf.Max((float) Screen.width / res.x,  1)),
                Mathf.FloorToInt(Mathf.Max((float) Screen.height / res.y, 1))
            );
        }
    }
}
