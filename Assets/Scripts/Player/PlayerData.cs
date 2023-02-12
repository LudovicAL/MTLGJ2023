using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : Singleton<PlayerData> {
    
    public UnityEvent<GameObject> playerVehicleChanged = new();
    
    [Tooltip("Minimum velocity magnitude required to kill a zombie")]
    public float murderSpeed = 10.0f;

    public PlayerStat ZombiesKilled = new();
    public PlayerStat Hp = new (10);
    public PlayerStat Xp = new ();
    public PlayerStat Fuel = new (100);

    //PlayerVehicle
    public GameObject playerVehicle { get; private set; }
    public void SetPlayerVehicle(GameObject go) {
        playerVehicle = go;
        playerVehicleChanged.Invoke(go);
    }
}
