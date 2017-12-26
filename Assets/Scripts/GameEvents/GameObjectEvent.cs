using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/GameObject")]
    public class GameObjectEvent : ScriptableObject {
        
        private readonly List<GameObjectEventListener> eventListeners = new List<GameObjectEventListener>();
        private Action<GameObject> callbacks = go => { };

        public void Invoke(GameObject go) {
            for (int i = eventListeners.Count - 1; i >= 0; i--) {
                eventListeners[i].OnEventRaised(go);
            }
            callbacks.Invoke(go);
        }

        public void RegisterListener(GameObjectEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameObjectEventListener listener) {
            eventListeners.Remove(listener);
        }

        public void RegisterCallback(Action<GameObject> callback) {
            callbacks -= callback;
            callbacks += callback;
        }

        public void UnregisterCallback(Action<GameObject> callback) {
            callbacks -= callback;
        }
    }
}
