using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {

    [Serializable]
    public class UnityVector3AndIntEvent : UnityEvent<Vector3, int> {}

    [AddComponentMenu("Game Events/Vector3 and Int Event Listener")]
    public class Vector3AndIntEventListener : MonoBehaviour {

        public Vector3AndIntEvent evt;
        public UnityVector3AndIntEvent response;

        private void OnEnable() {
            evt.AddListener(this);
        }

        private void OnDisable() {
            evt.RemoveListener(this);
        }

        public void OnEventInvoked(Vector3 payload1, int payload2) {
            response.Invoke(payload1, payload2);
        }
    }
}
