using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Vector2Int", fileName = "Vector2IntEvent.asset")]
public class Vector2IntEvent : ScriptableObject {
    private readonly List<Vector2IntEventListener> eventListeners = new List<Vector2IntEventListener>();

    public void Invoke(Vector2Int payload) {
        for (int i = eventListeners.Count -1; i >= 0; i--) {
            eventListeners[i].OnEventInvoked(payload);
        }
    }

    public void AddListener(Vector2IntEventListener listener) {
        if (!eventListeners.Contains(listener)) {
            eventListeners.Add(listener);
        }
    }

    public void RemoveListener(Vector2IntEventListener listener) {
        eventListeners.Remove(listener);
    }
}
