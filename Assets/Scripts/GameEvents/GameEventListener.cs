using UnityEngine;
using UnityEngine.Events;

namespace GameEvents {

    public class GameEventListener : MonoBehaviour {
        
        [Tooltip("Event to register with.")]
        public GameEvent gameEvent;

        [Tooltip("Response to invoke when gameEvent is raised.")]
        public UnityEvent response;

        private void OnEnable() {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised() {
            response.Invoke();
        }
    }
}
