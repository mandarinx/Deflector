using System;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    Idle          = 0,
    InMenu        = 1,
    InGame        = 2,
    InGameMuffled = 3,
    EndGame       = 4,
}

public enum Trigger {
    StartGame    = 0,
    LoadLevel    = 1,
    Play         = 2,
    Exit         = 3,
    ResetGame    = 4,
}

public class Transition {
    private readonly State   state;
    private readonly Trigger trigger;

    public Transition(State state, Trigger trigger) {
        this.state = state;
        this.trigger = trigger;
    }

    public override int GetHashCode() {
        return 17 + 31 * state.GetHashCode() + 31 * trigger.GetHashCode();
    }

    public override bool Equals(object obj) {
        Transition other = obj as Transition;
        return other != null && state == other.state && trigger == other.trigger;
    }
}

public class StateMachine {
    public State CurrentState { get; private set; }

    private readonly Dictionary<Transition, State> transitions;

    public StateMachine() {
        CurrentState = State.Idle;
        transitions = new Dictionary<Transition, State>();
    }

    public void AddTransition(State fromState, Trigger trigger, State toState) {
        transitions.Add(new Transition(fromState, trigger), toState);
    }

    public State GetNext(Trigger trigger) {
        Transition transition = new Transition(CurrentState, trigger);
        State nextState;
        if (!transitions.TryGetValue(transition, out nextState)) {
            throw new Exception($"Invalid transition from {CurrentState} via {trigger}");
        }
        return nextState;
    }

    public State MoveNext(Trigger trigger) {
        CurrentState = GetNext(trigger);
        return CurrentState;
    }
}

public class TestStateMachine : MonoBehaviour {

    private void Start() {
        StateMachine sm = new StateMachine();
        sm.AddTransition(State.Idle,          Trigger.StartGame, State.InMenu);
        sm.AddTransition(State.InMenu,        Trigger.LoadLevel, State.InGameMuffled);
        sm.AddTransition(State.InGameMuffled, Trigger.Play,      State.InGame);
        sm.AddTransition(State.InGame,        Trigger.Exit,      State.EndGame);
        sm.AddTransition(State.EndGame,       Trigger.ResetGame, State.InMenu);

        Debug.Log($"Current state: {sm.CurrentState}");
        sm.MoveNext(Trigger.StartGame);
        Debug.Log($"Trigger.StartGame: {sm.CurrentState}");
        sm.MoveNext(Trigger.Play);
        Debug.Log($"Trigger.Play: {sm.CurrentState}");
    }
}
