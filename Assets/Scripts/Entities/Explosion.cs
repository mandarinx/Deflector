using System.Collections;
using PowerTools;
using GameEvents;
using HyperGames.Lib;
using UnityEngine;

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
    private SpriteAnim[]  anims;

    public IEnumerator BigBadaBoom() {
        yield return new WaitForSeconds(Random.Range(delayMin, delayMax));

        onExplodedAt?.Invoke(transform.position);
        ArrayUtils.Shuffle(anims);
        Collider2D[] overlapped = Physics2D.OverlapCircleAll(transform.position,
                                                             radius,
                                                             layerExplode.value);

        for (int i = 0; i < overlapped.Length; ++i) {
            overlapped[i].tag = "HitByExplosion";
            overlapped[i]
               .GetComponent<Killable>()
              ?.Kill(transform.position);
        }

        int expl = 0;
        while (expl < anims.Length) {
            anims[expl].Play(anims[expl].Clip);
            ++expl;
            yield return new WaitForSeconds(Random.Range(0.03f, 0.09f));
        }

        yield return new WaitForSeconds(duration);
    }

    private void OnDrawGizmos() {
        GizmoUtils.DrawCircle(transform.position, radius, Color.white);
    }
}
