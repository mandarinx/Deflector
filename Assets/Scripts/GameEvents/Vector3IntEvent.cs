using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    [CreateAssetMenu(menuName = "Game Events/Vector3Int", fileName = "Vector3IntEvent.asset")]
    public class Vector3IntEvent : ScriptableObject {
        private readonly List<Vector3IntEventListener> eventListeners = new List<Vector3IntEventListener>();

        public void Invoke(Vector3Int payload) {
            for (int i = eventListeners.Count -1; i >= 0; i--) {
                eventListeners[i].OnEventInvoked(payload);
            }
        }

        public void AddListener(Vector3IntEventListener listener) {
            if (!eventListeners.Contains(listener)) {
                eventListeners.Add(listener);
            }
        }

        public void RemoveListener(Vector3IntEventListener listener) {
            eventListeners.Remove(listener);
        }
    }
}
