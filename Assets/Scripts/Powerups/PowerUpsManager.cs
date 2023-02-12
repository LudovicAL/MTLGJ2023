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
    private List<PowerUp> _availablePowerUps;
    private PowerUpController _powerUpController;

    private void OnEnable() {
        PlayerData.Instance.playerVehicleChanged.AddListener(GetAvailablePowerUps);
        GetAvailablePowerUps(PlayerData.Instance.playerVehicle);
    }
    
    private void OnDisable()
    {
        PlayerData.Instance.playerVehicleChanged.RemoveListener(GetAvailablePowerUps);
    }
    
    private void GetAvailablePowerUps(GameObject playerVehicle)
    {
        if (playerVehicle == null) {
            return;
        }
        
        _powerUpController = playerVehicle.GetComponentInChildren<PowerUpController>();
        _availablePowerUps = _powerUpController.AvailablePowers;

    }
    
    public void AcquirePowerUp(PowerUp powerUp) => powerUp.Acquire();
    

    public List<PowerUp> GetRandomPowerUps(int qty)
    {
        qty = Mathf.Min(qty, _availablePowerUps.Count);
        int[] indexes = Helpers.GetRandomNumbers(0, _availablePowerUps.Count, qty);

        return indexes.Select(index => _availablePowerUps[index]).ToList();
    }

    private enum PowerUpSelectionMode {
        Sequential,
        Random
    }
}
