using UnityEngine;

public class TriggerStay : MonoBehaviour {

    public bool stays;
    
    private void OnTriggerEnter2D(Collider2D other) {
        stays = true;
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        stays = true;
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        stays = false;
    }

}
