using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/Level", fileName = "LevelEvent.asset")]
    public class LevelEvent : ScriptableObject {
        private readonly List<LevelEventListener> eventListeners = new List<LevelEventListener>();

        public void Invoke(Level payload) {
            for (int i = eventListeners.Count -1; i >= 0; i--) {
                eventListeners[i].OnEventInvoked(payload);
            }
        }

        public void AddListener(LevelEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void RemoveListener(LevelEventListener listener) {
            eventListeners.Remove(listener);
        }
    }
}
