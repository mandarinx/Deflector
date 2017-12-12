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
    [Tooltip("Triggered when player presses S on the start panel")]
    public GameEvent     enterGameEvent;
    [Tooltip("Triggered when player presses R on the game over panel")]
    public GameEvent     resetGameEvent;
    public GameEvent     onButtonPress;

    private State _state;
    private State state {
        get { return _state; }
        set {
            switch (value) {
                case State.START:
                    EnterPanel(panelStart);
                    ClosePanel(panelIngame);
                    ClosePanel(panelGameOver);
                    _state = State.START;
                    return;
                case State.INGAME:
                    ClosePanel(panelStart);
                    EnterPanel(panelIngame);
                    ClosePanel(panelGameOver);
                    _state = State.INGAME;
                    return;
                default:
                    ClosePanel(panelStart);
                    ClosePanel(panelIngame);
                    EnterPanel(panelGameOver);
                    _state = State.GAMEOVER;
                    return;
            }
        }
    }
    
    private void Awake() {
        state = State.START;
    }

    public void OnPlayerDead() {
        state = State.GAMEOVER;
    }

    private void Update() {
        if (state == State.START) {
            if (Input.GetKeyUp(KeyCode.S)) {
                onButtonPress.Raise();
                state = State.INGAME;
                enterGameEvent.Raise();
            }
        }
        if (state == State.GAMEOVER) {
            if (Input.GetKeyUp(KeyCode.R)) {
                onButtonPress.Raise();
                state = State.INGAME;
                resetGameEvent.Raise();
            }
        }
    }

    private void EnterPanel(Component panel) {
        panel.gameObject.SetActive(true);
        panel.GetComponent<UIPanel>().OnEnter();
    }

    private void ClosePanel(Component panel) {
        panel.gameObject.SetActive(false);
        panel.GetComponent<UIPanel>().OnClose();
    }
}
