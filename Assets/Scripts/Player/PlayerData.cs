using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerData : Singleton<PlayerData> {
    
    public UnityEvent<GameObject> playerVehicleChanged = new();
    
    [Tooltip("Minimum velocity magnitude required to kill a zombie")]
    public float murderSpeed = 10.0f;

    public bool envCollisionDoDamage = true;
    
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

    public void ResetPlayerData() {
        playerVehicle = null;
        murderSpeed = 10.0f;
        ZombiesKilled = new();
        Hp = new(10);
        Xp = new();
        Fuel = new(100);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor {
    public override void OnInspectorGUI() {
        var playerData = (PlayerData)target;
        if (playerData == null) {
            return;
        }

        if (GUILayout.Button("Kill the player")) {
            PlayerData.Instance.Hp.Add(-PlayerData.Instance.Hp.Current);
        }

        DrawDefaultInspector();
    }
}
#endif
