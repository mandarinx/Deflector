using UnityEngine;

public class Master : MonoBehaviour {

    public GameObjectSet projectileSet;
    public GameObjectSet turretSet;

    public void OnPlayerDead() {
        turretSet.DespawnAll();
        projectileSet.DespawnAll();
    }

    public void OnDespawnProjectile(GameObject projectile) {
        projectileSet.Despawn(projectile);
    }

    public void OnDespawnTurret(GameObject turret) {
        turretSet.Despawn(turret);
    }
}
