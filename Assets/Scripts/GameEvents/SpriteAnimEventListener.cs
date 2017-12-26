using System;
using PowerTools;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {
    
    [Serializable]
    public class UnitySpriteAnimEvent : UnityEvent<SpriteAnim> {}

    public class SpriteAnimEventListener : MonoBehaviour {
    
        public SpriteAnimEvent evt;
        public UnitySpriteAnimEvent response;

        private void OnEnable() {
            evt.RegisterListener(this);
        }

        private void OnDisable() {
            evt.UnregisterListener(this);
        }

        public void OnEventRaised(SpriteAnim payload) {
            response.Invoke(payload);
        }
    }
}
