using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndedCanvas : Singleton<EndedCanvas> {
    [SerializeField]
    private TextMeshProUGUI textKillCount;
    [SerializeField]
    private TextMeshProUGUI textLevel;

    private void Start() {
        PlayerInputConveyer.Instance.playerInput.actions["Pause"].started += PauseKeyWasPressed;
    }

    private void OnEnable() {
        textKillCount.text = "Zombies killed: " + PlayerData.Instance.ZombiesKilled.Current;
        textLevel.text = "Level reached: " + PlayerLevelManager.Instance.currentLevel;
    }

    private void OnDisable() {
    }

    private void PauseKeyWasPressed(InputAction.CallbackContext callbackContext) {
        ResetGame();
    }

    public void ResetGame() {
        if (GameManaging.Instance.currentState == GameState.Ended) {
            //TODO: ALL SINGLETON ARE NOT CORRECTLY DESTROYED AT SCENELOAD. THIS AS TO BE CORRECTED.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
