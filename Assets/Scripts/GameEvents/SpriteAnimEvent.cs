using System;
using System.Collections.Generic;
using PowerTools;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/SpriteAnimEvent")]
    public class SpriteAnimEvent : ScriptableObject {
        
        private readonly List<SpriteAnimEventListener> eventListeners = new List<SpriteAnimEventListener>();
        private Action<SpriteAnim> callbacks = str => { };

        public void Invoke(SpriteAnim payload) {
            for (int i = eventListeners.Count - 1; i >= 0; i--) {
                eventListeners[i].OnEventRaised(payload);
            }
            callbacks.Invoke(payload);
        }

        public void RegisterListener(SpriteAnimEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(SpriteAnimEventListener listener) {
            eventListeners.Remove(listener);
        }

        public void RegisterCallback(Action<SpriteAnim> callback) {
            callbacks -= callback;
            callbacks += callback;
        }

        public void UnregisterCallback(Action<SpriteAnim> callback) {
            callbacks -= callback;
        }
    }
}
