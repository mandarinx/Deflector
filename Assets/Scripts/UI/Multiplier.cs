using System.Collections;
using GameEvents;
using UnityEngine;
using UnityEngine.UI;

public class Multiplier : MonoBehaviour {

    [SerializeField]
    private Text            label;
    [SerializeField]
    private float           moveSpeed = 1f;
    [SerializeField]
    private GameObjectEvent onWillDespawn;

    private float           targetY;

    private void OnEnable() {
        StopAllCoroutines();
    }

    public void Activate(Vector3 position, int value) {
        transform.position = position;
        targetY = position.y + 1f;
        label.text = $"{value}x";
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        while (transform.position.y < targetY) {
            transform.position += Vector3.up * (Time.deltaTime / moveSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        onWillDespawn?.Invoke(gameObject);
    }
}
