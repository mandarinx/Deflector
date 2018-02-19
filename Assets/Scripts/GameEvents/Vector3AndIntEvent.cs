using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/Vector3 and Int", fileName = "Vector3AndIntEvent.asset")]
    public class Vector3AndIntEvent : ScriptableObject {
        private readonly List<Vector3AndIntEventListener> eventListeners = new List<Vector3AndIntEventListener>();
        private Action<Vector3, int> callbacks = (v3, i) => { };

        public void Invoke(Vector3 payload1, int payload2) {
            for (int i = eventListeners.Count -1; i >= 0; i--) {
                eventListeners[i].OnEventInvoked(payload1, payload2);
            }
            callbacks.Invoke(payload1, payload2);
        }

        public void AddListener(Vector3AndIntEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void RemoveListener(Vector3AndIntEventListener listener) {
            eventListeners.Remove(listener);
        }

        public void AddCallback(Action<Vector3, int> callback) {
            callbacks -= callback;
            callbacks += callback;
        }

        public void RemoveCallback(Action<Vector3, int> callback) {
            callbacks -= callback;
        }
    }
}
