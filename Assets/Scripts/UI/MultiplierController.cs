using HyperGames;
using UnityEngine;

public class MultiplierController : MonoBehaviour {

    [SerializeField]
    private GameObject                 prefab;
    [SerializeField]
    private Transform                  parent;

    private GameObjectPool<Multiplier> pool;

    private void Awake() {
        pool = new GameObjectPool<Multiplier>(parent, prefab, 64, true);
        pool.Fill();
    }

    public void Despawn(GameObject projectile) {
        pool.Despawn(projectile.GetComponent<Multiplier>());
    }

    public void DespawnAll() {
        pool.Reset();
    }

    public void Spawn(Vector3 position, int value) {
        Multiplier multiplier;
        pool.Spawn(out multiplier);
        multiplier.Activate(position, value);
    }
}
