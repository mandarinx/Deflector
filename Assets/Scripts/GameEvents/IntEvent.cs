using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/Int", fileName = "IntEvent.asset")]
    public class IntEvent : ScriptableObject {
        private readonly List<IntEventListener> eventListeners = new List<IntEventListener>();

        public void Invoke(int payload) {
            for (int i = eventListeners.Count -1; i >= 0; i--) {
                eventListeners[i].OnEventInvoked(payload);
            }
        }

        public void AddListener(IntEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void RemoveListener(IntEventListener listener) {
            eventListeners.Remove(listener);
        }
    }
}
