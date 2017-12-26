using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Vector2Int", fileName = "Vector2IntEvent.asset")]
public class Vector2IntEvent : ScriptableObject {
    private readonly List<Vector2IntListener> eventListeners = new List<Vector2IntListener>();

    public void Invoke(Vector2Int payload) {
        for (int i = eventListeners.Count -1; i >= 0; i--) {
            eventListeners[i].OnEventInvoked(payload);
        }
    }

    public void AddListener(Vector2IntListener listener) {
        if (!eventListeners.Contains(listener)) {
            eventListeners.Add(listener);
        }
    }

    public void RemoveListener(Vector2IntListener listener) {
        eventListeners.Remove(listener);
    }
}
