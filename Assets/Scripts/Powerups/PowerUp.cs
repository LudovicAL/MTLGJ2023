using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp: MonoBehaviour
{
    public PowerUpData powerUpData;
    public GameObject model;
    public abstract void Acquire();
    public abstract void Execute();
    public abstract bool CanExecute();
}
