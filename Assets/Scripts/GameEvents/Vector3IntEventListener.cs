using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {

    [Serializable]
    public class UnityVector3IntEvent : UnityEvent<Vector3Int> {}

    [AddComponentMenu("Game Events/Vector3IntEventListener")]
    public class Vector3IntEventListener : MonoBehaviour {

        public Vector3IntEvent evt;
        public UnityVector3IntEvent response;

        private void OnEnable() {
            evt.AddListener(this);
        }

        private void OnDisable() {
            evt.RemoveListener(this);
        }

        public void OnEventInvoked(Vector3Int payload) {
            response.Invoke(payload);
        }
    }
}
