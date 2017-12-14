using System.Collections;
using PowerTools;
using GameEvents;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float         delayMin;
    public float         delayMax;
    public float         duration;
    public GameEvent     onExplode;
    public SpriteAnim[]  anims;
    
    public void Explode() {
        StartCoroutine(BigBadaBoom());
    }

    private IEnumerator BigBadaBoom() {
        yield return new WaitForSeconds(Random.Range(delayMin, delayMax));

        for (int i = 0; i < anims.Length; ++i) {
            anims[i].Play(anims[i].Clip);
        }

        onExplode.Invoke();

        int layer = 1 << LayerMask.NameToLayer("Projectiles") | 
                    1 << LayerMask.NameToLayer("Player");
        Collider2D[] overlapped = Physics2D.OverlapCircleAll(transform.position, 1.4f, layer);
        
        for (int i = 0; i < overlapped.Length; ++i) {
            overlapped[i].GetComponent<Killable>()?.Kill(transform.position);
        }
        
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
