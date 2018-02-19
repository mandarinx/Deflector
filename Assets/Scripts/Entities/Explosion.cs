using System.Collections;
using PowerTools;
using GameEvents;
using UnityEngine;

namespace LunchGame01 {
    public class Explosion : MonoBehaviour {

        [SerializeField]
        private float         delayMin;
        [SerializeField]
        private float         delayMax;
        [SerializeField]
        private float         duration;
        [SerializeField]
        private float         radius;
        [SerializeField]
        private LayerMask     layerExplode;
        [SerializeField]
        private Vector3Event  onExplodedAt;
        [SerializeField]
        private GameEvent     onChainReaction;
        [SerializeField]
        private SpriteAnim[]  anims;

        private Collider2D[] overlapped;

        private void OnEnable() {
            overlapped = new Collider2D[16];
        }

        public IEnumerator BigBadaBoom() {
            yield return new WaitForSeconds(Random.Range(delayMin, delayMax));

            onExplodedAt?.Invoke(transform.position);
            ArrayUtils.Shuffle(anims);

            int expl = 0;
            while (expl < anims.Length) {
                anims[expl].Play(anims[expl].Clip);
                ++expl;

                yield return new WaitForSeconds(Random.Range(0.03f, 0.09f));

                int num = Physics2D.OverlapCircleNonAlloc(transform.position,
                                                          radius,
                                                          overlapped,
                                                          layerExplode.value);

                for (int i = 0; i < num; ++i) {
                    if (overlapped[i].CompareTag("HitByExplosion")) {
                        continue;
                    }
                    onChainReaction.Invoke();
                    overlapped[i].tag = "HitByExplosion";
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
