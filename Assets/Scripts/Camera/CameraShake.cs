using UnityEngine;

namespace Deflector {
    [RequireComponent(typeof(Camera))]
    public class CameraShake : MonoBehaviour {

        [SerializeField]
        private CameraShakeConfig shakeConfig;

        private Camera            cam;
        private bool              active;

        private void Awake() {
            active = false;
            cam = GetComponent<Camera>();
        }

        public void Shake() {
            shakeConfig.Reset();
            active = true;
        }

        public void Stop() {
            shakeConfig.Finish();
        }

        public void StopImmediate() {
            shakeConfig.Finish(true);
        }

        private void LateUpdate() {
            if (!active) {
                return;
            }

            if (shakeConfig.IsDone()) {
                active = false;
                cam.ResetWorldToCameraMatrix();
            }

            // Camera always looks down the negative z-axis
            Matrix4x4 shakeMatrix = shakeConfig.ComputeMatrix()
                                    * Matrix4x4.TRS(Vector3.zero,
                                                    Quaternion.identity,
                                                    new Vector3(1, 1, -1));
            cam.worldToCameraMatrix = shakeMatrix * transform.worldToLocalMatrix;
        }
    }
}
