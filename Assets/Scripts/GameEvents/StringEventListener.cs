using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {
    
    [Serializable]
    public class UnityStringEvent : UnityEvent<string> {}

    public class StringEventListener : MonoBehaviour {
    
        public StringEvent stringEvent;
        public UnityStringEvent response;

        private void OnEnable() {
            stringEvent.RegisterListener(this);
        }

        private void OnDisable() {
            stringEvent.UnregisterListener(this);
        }

        public void OnEventRaised(string payload) {
            response.Invoke(payload);
        }
    }
}
