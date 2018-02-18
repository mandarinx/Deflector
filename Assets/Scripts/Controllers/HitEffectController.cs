using UnityEngine;

namespace LunchGame01 {
    public class HitEffectController : MonoBehaviour {

        [SerializeField]
        private GameObject                prefab;
        private GameObjectPool<HitEffect> pool;

        private void Awake() {
            pool = new GameObjectPool<HitEffect>(transform, prefab, 4, true);
            pool.Fill();
        }

        public void Spawn(GameObject go) {
            HitEffect shieldHitEffect;
            pool.Spawn(out shieldHitEffect);
            shieldHitEffect.transform.position = go.transform.position;
        }

        public void Despawn(GameObject instance) {
            pool.Despawn(instance.GetComponent<HitEffect>());
        }
    }
}
