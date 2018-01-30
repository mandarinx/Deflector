using UnityEngine;
using System.Collections;
using GameEvents;

public class Turret : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer  spawnHole;
    [SerializeField]
    private GameObjectEvent onDespawn;
    [SerializeField]
    private Vector3Event    onFire;

    private void OnEnable() {
        spawnHole.gameObject.SetActive(false);
        StartCoroutine(Fire());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private IEnumerator Fire() {
        const float blink = 0.25f;
        spawnHole.gameObject.SetActive(true);
        spawnHole.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(blink);
        spawnHole.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(blink);
        spawnHole.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(blink);
        spawnHole.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(blink);
        spawnHole.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(blink);
        spawnHole.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(blink);
        spawnHole.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(0.5f);
        
        onFire.Invoke(transform.position);
        
        yield return new WaitForSeconds(1f);
        onDespawn.Invoke(gameObject);
    }
}
