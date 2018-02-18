using UnityEngine;

namespace LunchGame01 {
    public class ScreenController : MonoBehaviour {

        [SerializeField]
        private PixelArtCamera paCam;

        [Tooltip("The amount of the screen to try to fill. 1 = fullscreen")]
        [Range(0f, 1f)]
        [SerializeField]
        private float          screenFill;

        private void Start() {
            // Use monitor resolution, not the window size.
            // Screen.width/height is the window size.
            Resolution res = Screen.currentResolution;
            Vector2Int fillRes = new Vector2Int(Mathf.FloorToInt(res.width * screenFill),
                                                Mathf.FloorToInt(res.height * screenFill));

            // Take HiDPI screens into account and double the scale.
            int baseScale = Screen.dpi > 160 ? 2 : 1;
            Vector2Int minRes = paCam.TargetResolution * baseScale;

            // Adjust the fill resolution to make sure it's
            // wide or high enough to contain the game.
            // The PixelArtCamera and ScreenSpaceTexture.shader
            // will make sure to render the game in the correct
            // aspect ratio, so we don't have to care about that.
            fillRes.x = Mathf.Max(fillRes.x, minRes.x);
            fillRes.y = Mathf.Max(fillRes.y, minRes.y);

            // Resize the game window.
            Screen.SetResolution(fillRes.x, fillRes.y, false, res.refreshRate);
        }
    }
}
