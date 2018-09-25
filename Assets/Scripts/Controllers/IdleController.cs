using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Deflector {

    public class IdleController : MonoBehaviour {

        [SerializeField]
        private IntAsset   timeout;
        [SerializeField]
        private UnityEvent onTimeout;

        private float      timeSinceInput;
        private bool       didDispatch;

        private void Update() {
            if (didDispatch) {
                return;
            }

            timeSinceInput += Time.deltaTime;

            if (InputController.anyKey) {
                timeSinceInput = 0f;
            }

            if (timeSinceInput < timeout.Value) {
                return;
            }

            onTimeout.Invoke();
            didDispatch = true;
        }

        /// <summary>
        /// Called by DemoController when demo video stops playing
        /// </summary>
        [UsedImplicitly]
        public void Restart() {
            didDispatch = false;
            timeSinceInput = 0f;
        }
    }
}
