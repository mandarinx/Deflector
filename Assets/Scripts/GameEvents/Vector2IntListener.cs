using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityVector2Int : UnityEvent<Vector2Int> {}

[AddComponentMenu("Game Events/Vector2IntListener")]
public class Vector2IntListener : MonoBehaviour {

    public Vector2IntEvent evt;
    public UnityVector2Int response;

    private void OnEnable() {
        evt.AddListener(this);
    }

    private void OnDisable() {
        evt.RemoveListener(this);
    }

    public void OnEventInvoked(Vector2Int payload) {
        response.Invoke(payload);
    }
}
