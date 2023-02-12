using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PowerUp: MonoBehaviour
{
    public UnityEvent<PowerUp> OnAcquire;
    
    public PowerUpData powerUpData;
    //TODO: grab children directly.
    public GameObject[] models;

    public virtual void Acquire()
    {
        OnAcquire?.Invoke(this);
    }

    public abstract void Execute();
    public abstract bool CanExecute();
}
