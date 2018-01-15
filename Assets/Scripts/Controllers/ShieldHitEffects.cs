using UnityEngine;

public class ShieldHitEffects : MonoBehaviour {

    [SerializeField]
    private GameObjectPool shieldHitEffects;
    
    public void Spawn(Vector3 position) {
        shieldHitEffects.Spawn(transform, position);
    }

    public void Despawn(GameObject instance) {
        shieldHitEffects.Despawn(instance);
    }
}
