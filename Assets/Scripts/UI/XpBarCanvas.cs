using System;
using UnityEngine;
using UnityEngine.UI;

public class XpBarCanvas : MonoBehaviour {

    private Image xpBarImage;

    private void Awake()
    {
        xpBarImage = GetComponent<Image>();
        xpBarImage.fillAmount = 0.0f;
    }

    private void OnEnable() {
        
        PlayerLevelManager.Instance.playerLeveledUpEvent.AddListener(PlayerLeveledUp);
        PlayerData.Instance.Xp.OnCurrentChanged.AddListener(XpChanged);
    }

    private void OnDisable()
    {
        PlayerLevelManager.Instance.playerLeveledUpEvent.RemoveListener(PlayerLeveledUp);
        PlayerData.Instance.Xp.OnCurrentChanged.RemoveListener(XpChanged);
    }

    private void PlayerLeveledUp() {

    }

    private void XpChanged(int currentXp) {
        xpBarImage.fillAmount = currentXp * 1.0f / (PlayerLevelManager.Instance.xpRequiredForNextLevel - PlayerLevelManager.Instance.xpAtStartOfLevel);
    }
    
}
