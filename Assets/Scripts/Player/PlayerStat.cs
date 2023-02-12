using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat
{
    //TODO: Make it generic at some point
    public PlayerStat(int max = -1)
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

    public void AddMax(int amount)
    {
        Max += amount;
        Add(amount);
    } 
}
