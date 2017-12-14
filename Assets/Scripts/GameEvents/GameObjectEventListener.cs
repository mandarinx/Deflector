using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {
    
    [Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject> {}

    public class GameObjectEventListener : MonoBehaviour {
    
        [Tooltip("Event to register with.")]
        public GameObjectEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityGameObjectEvent Response;

        private void OnEnable() {
            Event.RegisterListener(this);
        }

        private void OnDisable() {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(GameObject go) {
            Response.Invoke(go);
        }
    }
}
