using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {

    [Serializable]
    public class UnityLevelEvent : UnityEvent<Level> {}

    [AddComponentMenu("Game Events/LevelEventListener")]
    public class LevelEventListener : MonoBehaviour {

        public LevelEvent evt;
        public UnityLevelEvent response;

        private void OnEnable() {
            evt.AddListener(this);
        }

        private void OnDisable() {
            evt.RemoveListener(this);
        }

        public void OnEventInvoked(Level payload) {
            response.Invoke(payload);
        }
    }
}
