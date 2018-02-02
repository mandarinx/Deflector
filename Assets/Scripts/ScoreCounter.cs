using System.Collections;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

public class ScoreCounter : MonoBehaviour {

    [SerializeField]
    private IntAsset           score;
    [SerializeField]
    private IntAsset           multiplier;
    [SerializeField]
    private int                explosionBaseScore;
    [SerializeField]
    private float              multiplierCooldown;
    [SerializeField]
    private Vector3AndIntEvent onMultiplierIncreasedAt;

    private Coroutine          cooldown;

    /// <summary>
    /// Resets both multiplier and score.
    /// Called by OnGameReset handler.
    /// </summary>
    [UsedImplicitly]
    public void OnResetScore() {
        multiplier.SetValue(1);
        score.SetValue(0);
    }

    /// <summary>
    /// Increase score and multiplier based on the state of the passed projectile.
    /// Called by OnProjectileDespawn handler.
    /// </summary>
    /// <param name="projectileGO"></param>
    [UsedImplicitly]
    public void IncreaseScore(GameObject projectileGO) {
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
