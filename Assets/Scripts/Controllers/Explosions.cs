using UnityEngine;

public class Explosions : MonoBehaviour {
    
    [SerializeField]
    private GameObject explosionPrefab;

    public void Explode(Vector3 position) {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = position;
        explosion.GetComponent<Explosion>().Explode();
    }
}
