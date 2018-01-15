using System.Collections;
using GameEvents;
using UnityEngine;
using UnityEngine.UI;

public class HitPoint : MonoBehaviour {
    
    [SerializeField]
    private Text label;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private GameObjectEvent onWillDespawn;

    private float targetY;

    public void PositionAt(Vector3 pos) {
        transform.localPosition = pos;
        targetY = pos.y + 1f;
    }

    public void Show(int hitPoints) {
        label.text = $"-{hitPoints}";
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
