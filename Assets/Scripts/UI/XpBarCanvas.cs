using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBarCanvas : MonoBehaviour {

    private Image xpBarImage;

    // Start is called before the first frame update
    void Start() {
        xpBarImage = GetComponent<Image>();
        PlayerLevelManager.Instance.playerLeveledUpEvent.AddListener(PlayerLeveledUp);
        PlayerData.Instance.xpChanged.AddListener(XpChanged);
        xpBarImage.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void PlayerLeveledUp() {

    }

    private void XpChanged() {
        xpBarImage.fillAmount = ((PlayerData.Instance.xp - PlayerLevelManager.Instance.xpAtStartOfLevel) / PlayerLevelManager.Instance.xpRequiredForNextLevel);
    }
}
