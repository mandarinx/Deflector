using UnityEngine;

namespace Deflector {
    public class SetAnimatorTrigger : MonoBehaviour {

        [SerializeField]
        private string   trigger;
        [SerializeField]
        private Animator animator;

        public void SetTrigger() {
            animator.SetTrigger(trigger);
        }
    }
}
