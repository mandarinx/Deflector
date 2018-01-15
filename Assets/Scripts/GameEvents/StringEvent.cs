using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/String")]
    public class StringEvent : ScriptableObject {
        
        private readonly List<StringEventListener> eventListeners = new List<StringEventListener>();
        private Action<string> callbacks = str => { };

        public void Invoke(string payload) {
            for (int i = eventListeners.Count - 1; i >= 0; i--) {
                eventListeners[i].OnEventRaised(payload);
            }
            callbacks.Invoke(payload);
        }

        public void RegisterListener(StringEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(StringEventListener listener) {
            eventListeners.Remove(listener);
        }

        public void RegisterCallback(Action<string> callback) {
            callbacks -= callback;
            callbacks += callback;
        }

        public void UnregisterCallback(Action<string> callback) {
            callbacks -= callback;
        }
    }
}
