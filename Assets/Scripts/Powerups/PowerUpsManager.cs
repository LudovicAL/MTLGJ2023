using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PowerUpsManager : Singleton<PowerUpsManager> {

    [SerializeField]
    private PowerUpSelectionMode powerUpSelectionMode;
    private List<IPowerUp> _availablePowerUps;
    private PowerUpController _powerUpController;

    private void OnEnable() {
        PlayerData.Instance.playerVehicleChanged.AddListener(GetAvailablePowerUpsList);
        GetAvailablePowerUpsList(PlayerData.Instance.playerVehicle);
    }

    private void OnDisable()
    {
        PlayerData.Instance.playerVehicleChanged.RemoveListener(GetAvailablePowerUpsList);
    }

    public void ActivatePowerUp(PowerUpData powerUpData) {
        int index = 0;
        int max = _availablePowerUps.Count;
        for (; index < max; index++) {
            if (_availablePowerUps[index].PowerUpData == powerUpData) {
                break;
            }
        }
        if (index < max) {
            _availablePowerUps[index].go.SetActive(true);
            _availablePowerUps.RemoveAt(index);
        }
    }

    public List<PowerUpData> GetRandomPowerUps() {
        List<PowerUpData> scriptablePowerUps = new List<PowerUpData>();
        List<PowerUp> copyOfAvailablePowerUps = _availablePowerUps.ToList();
        for (int i = 0; i < 3 && copyOfAvailablePowerUps.Count > 0; i++) {
            int randomIndex = Random.Range(0, copyOfAvailablePowerUps.Count);
            scriptablePowerUps.Add(copyOfAvailablePowerUps[randomIndex].PowerUpData);
            copyOfAvailablePowerUps.RemoveAt(randomIndex);
        }
        return scriptablePowerUps;
    }

    private void GetAvailablePowerUpsList(GameObject playerVehicle)
    {
        if (playerVehicle == null) {
            return;
        }
        
        _powerUpController = playerVehicle.GetComponentInChildren<PowerUpController>();
        _availablePowerUps = _powerUpController.AvailablePowers;

    }

    private string GetAvailablePowerUpsAsString() {
        StringBuilder sb = new StringBuilder();
        for (int i = 0, max = _availablePowerUps.Count; i < max; i++) {
            if (i > 0) {
                sb.Append(", ");
            }
            sb.Append(_availablePowerUps[i].PowerUpData.name);
        }
        return sb.ToString();
    }

    private enum PowerUpSelectionMode {
        Sequential,
        Random
    }
}
