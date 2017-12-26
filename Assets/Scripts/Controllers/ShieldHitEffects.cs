using UnityEngine;

public class ShieldHitEffects : MonoBehaviour {

    public SpriteAnimSet shieldHitEffectSet;
    
    public void Spawn(Vector3 pos) {
        shieldHitEffectSet.Spawn(pos);
    }

    public void Despawn(GameObject hitEffect) {
        shieldHitEffectSet.Despawn(hitEffect);
    }
}
