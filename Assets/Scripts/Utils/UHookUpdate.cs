using UnityEngine;
using UnityEngine.Events;

namespace Deflector {
    public class UHookUpdate : MonoBehaviour, IOnUpdate {

        [SerializeField]
        private UHooks hooks;
        [SerializeField]
        private UnityEvent onUpdate;

        public void AddUpdate() {
            hooks.AddOnUpdate(this);
        }

        public void RemoveUpdate() {
            hooks.RemoveOnUpdate(this);
        }

        public void UOnUpdate() {
            onUpdate.Invoke();
        }
    }
}
