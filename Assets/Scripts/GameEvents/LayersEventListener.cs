using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {

    [Serializable]
    public class UnityLayersEvent : UnityEvent<Layers> {}

    [AddComponentMenu("Game Events/LayersEventListener")]
    public class LayersEventListener : MonoBehaviour {

        public LayersEvent evt;
        public UnityLayersEvent response;

        private void OnEnable() {
            evt.AddListener(this);
        }

        private void OnDisable() {
            evt.RemoveListener(this);
        }

        public void OnEventInvoked(Layers payload) {
            response.Invoke(payload);
        }
    }
}
