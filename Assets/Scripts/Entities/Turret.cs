using UnityEngine;
using System.Collections;
using RoboRyanTron.Unite2017.Events;

public class Turret : MonoBehaviour {

    public SpriteRenderer  spawnHole;
    public GameObjectSet   projectileSet;
    public GameObjectEvent onDespawn;
    public GameEvent       onFire;

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
        
        projectileSet.Spawn(transform.position);
        onFire.Raise();
        
        yield return new WaitForSeconds(1f);
        onDespawn.Raise(gameObject);
    }
}
