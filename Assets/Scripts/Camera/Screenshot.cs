using System;
using UnityEngine;

namespace Deflector {

    public class Screenshot : MonoBehaviour {

        [SerializeField]
        [Range(1, 10)]
        private int scale;
        [SerializeField]
        private KeyCode key;

        private void Update() {
            if (!Input.GetKeyUp(key)) {
                return;
            }
            int unixTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            ScreenCapture.CaptureScreenshot($"Screenshots/{unixTime}.png", scale);
        }
    }
}
