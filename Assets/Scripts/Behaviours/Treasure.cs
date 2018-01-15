using GameEvents;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Treasure : MonoBehaviour {
    
    [SerializeField]
    private Sprite spriteClosed;
    [SerializeField]
    private Sprite spriteOpenedGold;
    [SerializeField]
    private Sprite spriteOpenedEmpty;
    [SerializeField]
    private Sprite spriteOpenedTrap;
    [SerializeField]
    private HealthAsset health;
    [SerializeField]
    private Vector3AndIntEvent onTreasureHitAt;
    [SerializeField]
    private Vector3Event onTreasureDeadAt;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = spriteClosed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        onTreasureHitAt?.Invoke(transform.position, 1);
        health?.RemoveLives(1);
    }
    
    // Add support for player pickup
    // - switch to some other layer
    // - ontriggerenter => flip to open chest, give reward

    public void OnDead() {
        Destroy(gameObject);
        onTreasureDeadAt?.Invoke(transform.position);
    }
}
