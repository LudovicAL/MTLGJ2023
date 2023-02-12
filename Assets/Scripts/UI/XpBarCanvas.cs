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
        if (PlayerLevelManager.Instance != null) {
            PlayerLevelManager.Instance.playerLeveledUpEvent.RemoveListener(PlayerLeveledUp);
        }
        if (PlayerData.Instance != null) {
            PlayerData.Instance.Xp.OnCurrentChanged.RemoveListener(XpChanged);
        }
    }

    private void PlayerLeveledUp() {
        XpChanged(PlayerData.Instance.Xp.Current);
    }

    private void XpChanged(int currentXp) {
        xpBarImage.fillAmount = ((float)(currentXp - PlayerLevelManager.Instance.xpAtStartOfLevel)) / ((float)(PlayerLevelManager.Instance.xpRequiredForNextLevel - PlayerLevelManager.Instance.xpAtStartOfLevel));
    }
}
