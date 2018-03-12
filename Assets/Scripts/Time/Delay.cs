using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Deflector {
    public class Delay : MonoBehaviour {

        [SerializeField]
        private float      delay;
        [SerializeField]
        private UnityEvent onDelayed;

        public void StartDelay() {
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown() {
            yield return new WaitForSeconds(delay);
            onDelayed.Invoke();
        }
    }
}
