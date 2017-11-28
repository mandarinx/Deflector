using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class UIController : MonoBehaviour {

    private enum State {
        START = 0,
        INGAME = 1,
        GAMEOVER = 2
    }
    
    public RectTransform panelIngame;
    public RectTransform panelStart;
    public RectTransform panelGameOver;
    public PlayerHealth  playerHealth;
    [Tooltip("Triggered when player presses S on the start panel")]
    public GameEvent     enterGameEvent;
    [Tooltip("Triggered when player presses R on the game over panel")]
    public GameEvent     resetGameEvent;

    private State _state;
    private State state {
        get { return _state; }
        set {
            switch (value) {
                case State.START:
                    panelStart.gameObject.SetActive(true);
                    panelIngame.gameObject.SetActive(false);
                    panelGameOver.gameObject.SetActive(false);
                    _state = State.START;
                    return;
                case State.INGAME:
                    panelStart.gameObject.SetActive(false);
                    panelIngame.gameObject.SetActive(true);
                    panelGameOver.gameObject.SetActive(false);
                    _state = State.INGAME;
                    return;
                default:
                    panelStart.gameObject.SetActive(false);
                    panelIngame.gameObject.SetActive(false);
                    panelGameOver.gameObject.SetActive(true);
                    _state = State.GAMEOVER;
                    return;
            }
        }
    }
    
    private void Awake() {
        state = State.START;
        playerHealth.onDead = OnPlayerDead;
    }

    private void OnPlayerDead() {
        state = State.GAMEOVER;
    }

    private void Update() {
        if (state == State.START) {
            if (Input.GetKeyUp(KeyCode.S)) {
                state = State.INGAME;
                enterGameEvent.Raise();
            }
        }
        if (state == State.GAMEOVER) {
            if (Input.GetKeyUp(KeyCode.R)) {
                state = State.INGAME;
                resetGameEvent.Raise();
            }
        }
    }
}
