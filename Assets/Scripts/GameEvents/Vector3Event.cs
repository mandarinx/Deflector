using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/Vector3")]
    public class Vector3Event : ScriptableObject {
        
        private readonly List<Vector3EventListener> eventListeners = new List<Vector3EventListener>();
        private Action<Vector3> callbacks = str => { };

        public void Invoke(Vector3 payload) {
            for (int i = eventListeners.Count - 1; i >= 0; i--) {
                eventListeners[i].OnEventRaised(payload);
            }
            callbacks.Invoke(payload);
        }

        public void RegisterListener(Vector3EventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(Vector3EventListener listener) {
            eventListeners.Remove(listener);
        }

        public void RegisterCallback(Action<Vector3> callback) {
            callbacks -= callback;
            callbacks += callback;
        }

        public void UnregisterCallback(Action<Vector3> callback) {
            callbacks -= callback;
        }
    }
}
