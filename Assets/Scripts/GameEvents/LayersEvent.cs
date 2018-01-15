using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/Layers", fileName = "LayersEvent.asset")]
    public class LayersEvent : ScriptableObject {
        private readonly List<LayersEventListener> eventListeners = new List<LayersEventListener>();

        public void Invoke(Layers payload) {
            for (int i = eventListeners.Count -1; i >= 0; i--) {
                eventListeners[i].OnEventInvoked(payload);
            }
        }

        public void AddListener(LayersEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void RemoveListener(LayersEventListener listener) {
            eventListeners.Remove(listener);
        }
    }
}
