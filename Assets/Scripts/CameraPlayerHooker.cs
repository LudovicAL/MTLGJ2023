using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerHooker : MonoBehaviour {

    CinemachineVirtualCamera cinemachineVirtualCamera;

    // Start is called before the first frame update
    void Start() {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (PlayerData.Instance.playerVehicle != null) {
            PlayerVehiculeChanged(PlayerData.Instance.playerVehicle);
        }
    }

    private void OnEnable() {
        PlayerData.Instance.playerVehicleChanged.AddListener(PlayerVehiculeChanged);
    }

    private void OnDisable() {
        PlayerData.Instance?.playerVehicleChanged.RemoveListener(PlayerVehiculeChanged);
    }

    private void PlayerVehiculeChanged(GameObject playerVehicle) {
        cinemachineVirtualCamera.Follow = playerVehicle.transform;
        cinemachineVirtualCamera.LookAt = playerVehicle.transform;
    }
}
