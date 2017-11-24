using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    public SpriteRenderer warning;
    public SpriteRenderer cannon;
    public GameObject projectile;

    private void Awake() {
        warning.gameObject.SetActive(true);
        cannon.gameObject.SetActive(false);
    }

    private void Start() {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire() {
        warning.color = new Color(0f, 0f, 0f, 0.25f);
        yield return new WaitForSeconds(0.5f);
        warning.color = new Color(0f, 0f, 0f, 0.35f);
        yield return new WaitForSeconds(0.4f);
        warning.color = new Color(0f, 0f, 0f, 0.45f);
        yield return new WaitForSeconds(0.3f);
        warning.color = new Color(0f, 0f, 0f, 0.6f);
        yield return new WaitForSeconds(0.2f);
        warning.color = new Color(0f, 0f, 0f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        warning.color = new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.1f);

        warning.gameObject.SetActive(false);
        cannon.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);

        GameObject go = Instantiate(projectile);
        go.transform.position = transform.position;
        
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
