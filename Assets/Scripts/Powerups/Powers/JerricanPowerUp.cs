using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerricanPowerUp : MonoBehaviour, IPowerUp
{
    [SerializeField] private int fuelBoost = 25;
    [SerializeField] private GameObject model;

    public bool IsPassive { get; private set; }
    public bool IsAcquired { get; private set;}

    public void Acquire()
    {
        IsAcquired = true;
        IsPassive = true;
        
        model.SetActive(true);
        Execute();
    }

    public void Execute()
    {
        PlayerData.Instance.Fuel.AddMax(fuelBoost);
    }

    public bool CanExecute()
    {
        return true;
    }
}
