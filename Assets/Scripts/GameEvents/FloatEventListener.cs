using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityFloatEvent : UnityEvent<float> {}

[AddComponentMenu("Game Events/FloatEventListener")]
public class FloatEventListener : MonoBehaviour {

    public FloatEvent evt;
    public UnityFloatEvent response;

    private void OnEnable() {
        evt.AddListener(this);
    }

    private void OnDisable() {
        evt.RemoveListener(this);
    }

    public void OnEventInvoked(float payload) {
        response.Invoke(payload);
    }
}
