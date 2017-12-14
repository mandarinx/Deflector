using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {
    
    [CreateAssetMenu(menuName = "Game Events/GameEvent")]
    public class GameEvent : ScriptableObject {

        private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();
        private Action callbacks = () => { };

        public void Invoke() {
            for (int i = eventListeners.Count - 1; i >= 0; i--) {
                eventListeners[i].OnEventRaised();
            }
            callbacks.Invoke();
        }

        public void RegisterListener(GameEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener) {
            eventListeners.Remove(listener);
        }

        public void RegisterCallback(Action callback) {
            callbacks -= callback;
            callbacks += callback;
        }

        public void UnregisterCallback(Action callback) {
            callbacks -= callback;
        }
    }
}
