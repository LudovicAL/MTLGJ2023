using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIController : MonoBehaviour
{
    public TMP_Text fuelDisplay;
    public TMP_Text hpDisplay;
    
    private void OnEnable()
    {
        UpdateHpDisplay(PlayerData.Instance.Hp.Current);
        UpdateFuelDisplay(PlayerData.Instance.Fuel.Current);
        
        PlayerData.Instance.Fuel.OnCurrentChanged.AddListener(UpdateFuelDisplay);
        PlayerData.Instance.Hp.OnCurrentChanged.AddListener(UpdateHpDisplay);
    }
    
    private void OnDisable()
    {
        PlayerData.Instance.Fuel.OnCurrentChanged.RemoveListener(UpdateFuelDisplay);
        PlayerData.Instance.Hp.OnCurrentChanged.RemoveListener(UpdateHpDisplay);
    }

    private void UpdateHpDisplay(int currentHp)
    {
        hpDisplay.text = $"Hp -> {currentHp}/{PlayerData.Instance.Hp.Max}";
    }

    private void UpdateFuelDisplay(int currentGaz)
    {
        fuelDisplay.text = $"Fuel -> {currentGaz}/{PlayerData.Instance.Fuel.Max}";
    }
}
