using System.Collections;
using GameEvents;
using UnityEngine;

namespace Deflector {
    public class Explosion : MonoBehaviour {

        [SerializeField]
        private float              delayMin;
        [SerializeField]
        private float              delayMax;
        [SerializeField]
        private float              duration;
        [SerializeField]
        private float              radius;
        [SerializeField]
        private LayerMask          layerExplode;
        [SerializeField]
        private Vector3Event       onExplodedAt;
        [SerializeField]
        private GameEvent          onChainReaction;
        [SerializeField]
        private SpriteAnimPlayer[] anims;

        private Collider2D[]       overlapped;
        private const string       hitByExplosionTag = "HitByExplosion";

        private void OnEnable() {
            overlapped = new Collider2D[16];
        }

        [ContextMenu("Explode!")]
        private void Explode() {
            StartCoroutine(BigBadaBoom());
        }

        public IEnumerator BigBadaBoom() {
            yield return new WaitForSeconds(Random.Range(delayMin, delayMax));

            onExplodedAt?.Invoke(transform.position);
            ArrayUtils.Shuffle(anims);

            int expl = 0;
            while (expl < anims.Length) {
                anims[expl].Play();
                ++expl;

                yield return new WaitForSeconds(Random.Range(0.03f, 0.09f));

                int num = Physics2D.OverlapCircleNonAlloc(transform.position,
                                                          radius,
                                                          overlapped,
                                                          layerExplode.value);

                for (int i = 0; i < num; ++i) {
                    if (overlapped[i].CompareTag(hitByExplosionTag)) {
                        continue;
                    }
                    onChainReaction.Invoke();
                    overlapped[i].tag = hitByExplosionTag;
                    overlapped[i]
                       .GetComponent<Killable>()
                      ?.Kill(transform.position);
                }
            }

            yield return new WaitForSeconds(duration);
        }

        private void OnDrawGizmos() {
            GizmoUtils.DrawCircle(transform.position, radius, Color.white);
        }
    }
}
