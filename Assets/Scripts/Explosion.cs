using System.Collections;
using PowerTools;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float         delayMin;
    public float         delayMax;
    public float         duration;
    public SpriteAnim[]  anims;
    
    public void Explode() {
        StartCoroutine(BigBadaBoom());
    }

    private IEnumerator BigBadaBoom() {
        yield return new WaitForSeconds(Random.Range(delayMin, delayMax));

        for (int i = 0; i < anims.Length; ++i) {
            anims[i].Play(anims[i].Clip);
        }

        Collider2D[] overlapped = Physics2D.OverlapCircleAll(transform.position, 1.4f, 1 << LayerMask.NameToLayer("Projectiles"));
        
        for (int i = 0; i < overlapped.Length; ++i) {
            overlapped[i].GetComponent<Projectile>().Explode();
        }
        
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
