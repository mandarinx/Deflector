using UnityEngine;

namespace Deflector {
    public class HitEffectController : MonoBehaviour {

        [SerializeField]
        private GameObject                prefab;
        private GameObjectPool<HitEffect> pool;

        private void Awake() {
            pool = new GameObjectPool<HitEffect>(transform, prefab, 4, true);
            pool.Fill();
        }

        public void Spawn(GameObject go) {
            HitEffect hitEffect;
            pool.Spawn(out hitEffect);
            hitEffect.transform.position = go.transform.position;
            hitEffect.Activate();
        }

        public void Despawn(GameObject instance) {
            pool.Despawn(instance.GetComponent<HitEffect>());
        }
    }
}
