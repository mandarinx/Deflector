using UnityEngine;
using UnityEngine.Events;

namespace Deflector {
    public class InputKeyTrigger : MonoBehaviour {

        [SerializeField]
        private KeyCode    keyCode;
        [SerializeField]
        private UnityEvent onInput;

        public void CheckInputKey() {
            if (Input.GetKeyUp(keyCode)) {
                onInput.Invoke();
            }
        }
    }
}
