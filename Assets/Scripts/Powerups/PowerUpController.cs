using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private List<IPowerUp> _powerUps;

    public List<IPowerUp> AcquiredPowers = new();
    public List<IPowerUp> AvailablePowers = new();
    
    private void Awake()
    {
        _powerUps = GetComponentsInChildren<IPowerUp>().ToList();
        AvailablePowers = _powerUps;
    }
    
    
    
}
