using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIDebugger : MonoBehaviour
{
    public TMP_Text fuelDisplay;
    public TMP_Text hpDisplay;
    public TMP_Text murderSpeed;
    public TMP_Text ZombiesKilled;
    
    private void OnEnable()
    {
        UpdateHpDisplay(PlayerData.Instance.Hp.Current);
        UpdateFuelDisplay(PlayerData.Instance.Fuel.Current);
        UpdateZombiesDisplay(PlayerData.Instance.ZombiesKilled.Current);
        
        PlayerData.Instance.Fuel.OnCurrentChanged.AddListener(UpdateFuelDisplay);
        PlayerData.Instance.Hp.OnCurrentChanged.AddListener(UpdateHpDisplay);
        PlayerData.Instance.ZombiesKilled.OnCurrentChanged.AddListener(UpdateZombiesDisplay);
    }

    private void OnDisable()
    {
        PlayerData.Instance.Fuel.OnCurrentChanged.RemoveListener(UpdateFuelDisplay);
        PlayerData.Instance.Hp.OnCurrentChanged.RemoveListener(UpdateHpDisplay);
        PlayerData.Instance.ZombiesKilled.OnCurrentChanged.RemoveListener(UpdateZombiesDisplay);
    }

    private void UpdateHpDisplay(int currentHp)
    {
        hpDisplay.text = $"Hp -> {currentHp}/{PlayerData.Instance.Hp.Max}";
    }

    private void UpdateFuelDisplay(int currentGaz)
    {
        fuelDisplay.text = $"Fuel -> {currentGaz}/{PlayerData.Instance.Fuel.Max}";
    }
    
    private void UpdateZombiesDisplay(int zombiesKilled)
    {
        ZombiesKilled.text = $"Zombies killed -> {zombiesKilled}";
    }
}
