using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : Singleton<PlayerData> {
    
    public UnityEvent<GameObject> playerVehicleChanged = new();
    
    public int zombiesKilled = 0;
    [Tooltip("Minimum velocity magnitude required to kill a zombie")]
    public float murderSpeed = 10.0f;

    public Stats Hp;
    public Stats Xp;
    public Stats Fuel;

    //PlayerVehicle
    public GameObject playerVehicle { get; private set; }
    public void SetPlayerVehicle(GameObject go) {
        playerVehicle = go;
        playerVehicleChanged.Invoke(go);
    }

    private void Awake()
    {
        Hp = new Stats(10);
        Xp = new Stats();
        Fuel = new Stats(100);
    }
}

public class Stats
{
    public Stats(int max = -1)
    {
        Max = max;
        Current = Mathf.Max(0, Max);
    }

    public UnityEvent<int> OnCurrentChanged = new();
    public UnityEvent<int> OnMaxChanged = new();

    private int _max;
    public int Max
    {
        get => _max;
        private set
        {
            _max = value;
            OnMaxChanged?.Invoke(value);
        }
    }
    
    private int _current;
    public int Current
    {
        get => _current;
        private set
        {
            _current = value;
            OnCurrentChanged?.Invoke(value);
        }
    }

    public void Add(int amount) => Current = (Max >= 0) ? Mathf.Min(Current + amount, Max) : Current + amount;
    public void Reset() => Current = 0;
    public void AddMax(int amount) => Max += amount;
}
