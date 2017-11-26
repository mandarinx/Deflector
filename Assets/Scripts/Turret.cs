using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    public SpriteRenderer spawnHole;
    public GameObject     projectile;

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

        GameObject go = Instantiate(projectile);
        go.transform.position = transform.position;
        
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
