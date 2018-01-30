using UnityEngine;
using GameEvents;
using PowerTools;

public class ShieldHitEffect : MonoBehaviour {
    
    [SerializeField]
    private GameObjectEvent onShieldEffectDone;
    private SpriteAnim      anim;

    private void Awake() {
        anim = GetComponent<SpriteAnim>();
    }

    private void OnEnable() {
        anim.SetTime(0f);
        anim.Play(anim.Clip);
    }

    public void OnAnimDone() {
        onShieldEffectDone.Invoke(gameObject);
    }
}
