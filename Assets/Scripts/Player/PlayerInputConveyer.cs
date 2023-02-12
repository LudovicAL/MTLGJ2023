using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputConveyer : Singleton<PlayerInputConveyer> {
    public PlayerInput playerInput { get; private set; }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }
}
