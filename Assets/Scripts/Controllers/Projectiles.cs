using GameEvents;
using UnityEngine;

public class Projectiles : MonoBehaviour {

    public GameObjectPool projectilePool;
    public Vector3Event   onProjectileExplodedAt;

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

    public void Spawn(Vector3 pos) {
        if (!active) {
            return;
        }
        projectilePool.Spawn(pos);
    }

    public void Despawn(GameObject projectile) {
        projectilePool.Despawn(projectile);
        onProjectileExplodedAt.Invoke(projectile.transform.position);
    }
}
