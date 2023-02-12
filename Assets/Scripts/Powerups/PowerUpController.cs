using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private List<PowerUp> _powerUps;

    public List<PowerUp> AcquiredPowers = new();
    public List<PowerUp> AvailablePowers = new();
    
    private void Awake()
    {
        _powerUps = GetComponentsInChildren<PowerUp>().ToList();
        AvailablePowers = _powerUps;
    }
    
    
    
}
