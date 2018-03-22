using UnityEngine;

namespace Deflector {

    [CreateAssetMenu(menuName = "Config/Sprite Anim Shared Controller", fileName = "SpriteAnimSharedController.asset")]
    public class SpriteAnimSharedController : ScriptableObject {
        [SerializeField]
        private RuntimeAnimatorController  sharedAnimController;
        [SerializeField]
        private string                     stateName;

        public RuntimeAnimatorController Controller => sharedAnimController;
        public string                    StateName => stateName;

    }
}
