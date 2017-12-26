using UnityEngine;
using GameEvents;

public class ShieldHitEffect : MonoBehaviour {

    public GameObjectEvent onShieldEffectDone;
    
    public void OnAnimDone() {
        onShieldEffectDone.Invoke(gameObject);
    }
}
