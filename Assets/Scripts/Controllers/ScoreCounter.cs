using System.Collections;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;

namespace Deflector {
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
        [SerializeField]
        private GameEvent          onMultiplierDecreased;

        private Coroutine          cooldown;

        /// <summary>
        /// Resets both multiplier and score.
        /// Called by OnGameReset handler.
        /// </summary>
        [UsedImplicitly]
        public void ResetScore() {
            multiplier.SetValue(1);
            score.SetValue(0);
        }

        /// <summary>
        /// Increase score and multiplier based on the state of the passed projectile.
        /// Called by OnProjectileExploded handler.
        /// </summary>
        /// <param name="projectileGO"></param>
        [UsedImplicitly]
        public void IncreaseScore(GameObject projectileGO) {
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            if (projectile == null) {
                return;
            }

            score.SetValue(score.Value + (explosionBaseScore * multiplier.Value));

            int increase = projectile.IsCharged &&
                           projectile.CompareTag("HitByExplosion") ? 1 : 0;
            multiplier.SetValue(multiplier.Value + increase);
            if (increase > 0) {
                onMultiplierIncreasedAt.Invoke(projectileGO.transform.position, multiplier.Value);
            }

            if (cooldown != null) {
                StopCoroutine(cooldown);
            }
            cooldown = StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown() {
            while (multiplier.Value > 1) {
                yield return new WaitForSeconds(multiplierCooldown);
                multiplier.SetValue(multiplier.Value - 1);
                onMultiplierDecreased.Invoke();
            }
        }
    }
}
