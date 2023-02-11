using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject player;
    public GameState currentState;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        //TODO: should probably listen to the player spawner but we dont have a player spawner yet.
        player = GameObject.FindWithTag("Player");

        //TODO: this is also bad. This whole script is bad atm. The Zombie state machine needs to know the game state tho.
        currentState = GameState.Started;
    }
}

public enum GameState {
    Menu,
    Started,
    Paused,
    Ended
}