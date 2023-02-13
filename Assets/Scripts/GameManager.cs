using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager> {

    public UnityEvent gameStateChangedEvent { get; private set; } = new UnityEvent();

    public GameState currentState { get; private set; } = GameState.Menu;

    private void Awake() {
        Application.targetFrameRate = 60;
    }

    public void RequestGameStateChange(GameState newGameState) {
        currentState = newGameState;
        Time.timeScale = (newGameState == GameState.Started || newGameState == GameState.Starting) ? 1 : 0;
        gameStateChangedEvent.Invoke();
    }
}

public enum GameState {
    Menu,
    Starting,
    Started,
    Paused,
    PowerUp,
    Ended
}