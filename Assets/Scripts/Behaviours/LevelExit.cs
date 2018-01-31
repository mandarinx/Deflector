using GameEvents;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelExit : MonoBehaviour {

    [SerializeField]
    private Sprite     openDoor;
    [SerializeField]
    private Sprite     closedDoor;

    [Header("Events Out")]
    [SerializeField]
    private GameEvent  onLevelExit;

    private SpriteRenderer sr;

    private void OnEnable() {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedDoor;
    }

    public void OpenDoor() {
        sr.sprite = openDoor;
    }

    public void CloseDoor() {
        sr.sprite = closedDoor;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        onLevelExit.Invoke();
    }
}
