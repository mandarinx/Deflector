using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {

    [Serializable]
    public class UnityIntEvent : UnityEvent<int> {}

    [AddComponentMenu("Game Events/IntEventListener")]
    public class IntEventListener : MonoBehaviour {

        public IntEvent evt;
        public UnityIntEvent response;

        private void OnEnable() {
            evt.AddListener(this);
        }

        private void OnDisable() {
            evt.RemoveListener(this);
        }

        public void OnEventInvoked(int payload) {
            response.Invoke(payload);
        }
    }
}
