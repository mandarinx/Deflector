using HyperGames;
using UnityEngine;

public class ShieldHitEffectController : MonoBehaviour {

    [SerializeField]
    private GameObject                      prefab;
    private GameObjectPool<ShieldHitEffect> pool;

    private void Awake() {
        pool = new GameObjectPool<ShieldHitEffect>(transform, prefab, 4, true);
        pool.Fill();
    }

    public void Spawn(GameObject go) {
        ShieldHitEffect shieldHitEffect;
        pool.Spawn(out shieldHitEffect);
        shieldHitEffect.transform.position = go.transform.position;
    }

    public void Despawn(GameObject instance) {
        pool.Despawn(instance.GetComponent<ShieldHitEffect>());
    }
}
