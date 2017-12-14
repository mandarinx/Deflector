using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {
    
    [Serializable]
    public class UnityVector3Event : UnityEvent<Vector3> {}

    public class Vector3EventListener : MonoBehaviour {
    
        public Vector3Event vector3Event;
        public UnityVector3Event response;

        private void OnEnable() {
            vector3Event.RegisterListener(this);
        }

        private void OnDisable() {
            vector3Event.UnregisterListener(this);
        }

        public void OnEventRaised(Vector3 payload) {
            response.Invoke(payload);
        }
    }
}
