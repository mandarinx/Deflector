using UnityEngine;

public class Projectiles : MonoBehaviour {

    public GameObjectSet projectileSet;

    private bool active;

    private void Awake() {
        active = false;
    }

    public void Activate() {
        active = true;
    }

    public void Deactivate() {
        active = false;
    }

    public void SpawnProjectile(Vector3 pos) {
        if (!active) {
            return;
        }
        projectileSet.Spawn(pos);
    }
}
