using System.Collections;
using GameEvents;
using UnityEngine;

public class ScoreCounter : MonoBehaviour {

    public IntAsset score;
    public IntAsset multiplier;
    public int      explosionBaseScore;
    public float    multiplierCooldown;
    
    [SerializeField]
    private Vector3AndIntEvent onMultiplierIncreasedAt;
    
    private Coroutine cooldown;

    public void OnResetScore() {
        multiplier.SetValue(1);
        score.SetValue(0);
    }

    public void OnProjectileDespawn(GameObject projectileGO) {
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile == null) {
            return;
        }

        score.SetValue(score.value + (explosionBaseScore * multiplier.value));

        int increase = projectile.isActivated && projectile.tag.Contains("HitByExplosion") ? 1 : 0;
        multiplier.SetValue(multiplier.value + increase);
        if (increase > 0) {
            onMultiplierIncreasedAt.Invoke(projectileGO.transform.position, multiplier.value);
        }
        
        if (cooldown != null) {
            StopCoroutine(cooldown);
        }
        cooldown = StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown() {
        while (multiplier.value > 1) {
            yield return new WaitForSeconds(multiplierCooldown);
            multiplier.SetValue(multiplier.value - 1);
        }
    }
}
