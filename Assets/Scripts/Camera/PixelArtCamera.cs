using UnityEngine;

namespace LunchGame01 {

    public class PixelArtCamera : MonoBehaviour {

        [SerializeField]
        private Vector2Int    targetRes;
        [SerializeField]
        private Material      rtMaterial;

        private RenderTexture rt;
        private Camera        cam;

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
            int offsetX = Mathf.FloorToInt((Screen.width - width) * 0.5f);
            int offsetY = Mathf.FloorToInt((Screen.height - height) * 0.5f);

            rtMaterial.SetVector("_Pos", new Vector4(offsetX,
                                                     offsetY,
                                                     offsetX + width,
                                                     offsetY + height));
            rtMaterial.SetTexture("_MainTex", rt);

            Graphics.Blit(rt, null, rtMaterial);
        }

        private static int BestFitScale(Vector2Int res) {
            return Mathf.Min(
                Mathf.FloorToInt(Mathf.Max((float) Screen.width / res.x,  1)),
                Mathf.FloorToInt(Mathf.Max((float) Screen.height / res.y, 1))
            );
        }
    }
}
