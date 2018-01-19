using UnityEngine;

public class Killer : MonoBehaviour {

    [SerializeField]
    private GameObjectPool pool;
    [SerializeField]
    private LayerMask      layer;

    private void OnTriggerEnter2D(Collider2D other) {
        if ((layer.value & 1 << other.gameObject.layer) == 0) {
            return;
        }
        pool.Despawn(other.gameObject);
    }
}
