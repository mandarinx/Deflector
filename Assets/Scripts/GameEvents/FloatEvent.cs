using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Float", fileName = "FloatEvent.asset")]
public class FloatEvent : ScriptableObject {
    private readonly List<FloatEventListener> eventListeners = new List<FloatEventListener>();

    public void Invoke(float payload) {
        for (int i = eventListeners.Count -1; i >= 0; i--) {
            eventListeners[i].OnEventInvoked(payload);
        }
    }

    public void AddListener(FloatEventListener listener) {
        if (!eventListeners.Contains(listener)) {
            eventListeners.Add(listener);
        }
    }

    public void RemoveListener(FloatEventListener listener) {
        eventListeners.Remove(listener);
    }
}
