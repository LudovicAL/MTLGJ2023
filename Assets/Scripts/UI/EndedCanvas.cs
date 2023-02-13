using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndedCanvas : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI textKillCount;
    [SerializeField]
    private TextMeshProUGUI textLevel;



    private void OnEnable() {
        textKillCount.text = "Zombies killed: " + PlayerData.Instance.ZombiesKilled.Current;
        textLevel.text = "Level reached: " + PlayerLevelManager.Instance.currentLevel;
    }

    public void ResetGame() {
        Debug.Log("TODO: A GAME RESET MECANISM");
    }
}
