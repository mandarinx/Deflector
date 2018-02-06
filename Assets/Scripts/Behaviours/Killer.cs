using GameEvents;
using UnityEngine;

public class Killer : MonoBehaviour {

    [SerializeField]
    private GameObjectEvent onKilled;
    [SerializeField]
    private LayerMask       layer;

    private void OnTriggerEnter2D(Collider2D other) {
        if ((layer.value & 1 << other.gameObject.layer) == 0) {
            return;
        }
        onKilled.Invoke(other.gameObject);
    }

    public void SetOnKilledEvent(GameObjectEvent evt) {
        onKilled = evt;
    }

    public void SetLayerMask(LayerMask layerMask) {
        layer = layerMask;
    }
}
