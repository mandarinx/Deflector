using System.Collections;
using PowerTools;
using GameEvents;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [SerializeField]
    private float         delayMin;
    [SerializeField]
    private float         delayMax;
    [SerializeField]
    private float         duration;
    [SerializeField]
    private LayerMask     layerExplode;
    [SerializeField]
    private Vector3Event  onExplodedAt;
    [SerializeField]
    private SpriteAnim[]  anims;
    
    public IEnumerator BigBadaBoom() {
        yield return new WaitForSeconds(Random.Range(delayMin, delayMax));

        for (int i = 0; i < anims.Length; ++i) {
            anims[i].Play(anims[i].Clip);
        }

        onExplodedAt?.Invoke(transform.position);
        
        Collider2D[] overlapped = Physics2D.OverlapCircleAll(transform.position, 
                                                             1.4f, 
                                                             layerExplode.value);
        
        for (int i = 0; i < overlapped.Length; ++i) {
            overlapped[i].tag = "HitByExplosion";
            overlapped[i]
               .GetComponent<Killable>()
              ?.Kill(transform.position);
        }
        
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
