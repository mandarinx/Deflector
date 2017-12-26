using UnityEngine;
using System.Collections;
using GameEvents;

public class Turret : MonoBehaviour {

    public SpriteRenderer  spawnHole;
    public GameObjectEvent onDespawn;
    public Vector3Event    onFire;

    private void Awake() {
        spawnHole.gameObject.SetActive(false);
    }

    private void Start() {
        StartCoroutine(Fire());
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
