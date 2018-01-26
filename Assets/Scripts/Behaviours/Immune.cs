using UnityEngine;

[RequireComponent(typeof(Player))]
public class Immune : MonoBehaviour {

    public bool immune;

    private Player player;
    
    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        player.SetImmune(immune);
    }
}
