using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;

public class PlayerLevelManager : Singleton<PlayerLevelManager> {

    public UnityEvent playerLeveledUpEvent = new UnityEvent();

    [SerializeField]
    private float constant;

    public int currentLevel { get; private set; } = 0;
    public int xpAtStartOfLevel { get; private set; } = 0;
    public int xpRequiredForNextLevel { get; private set; } = int.MaxValue;

    private void Start() {
        xpRequiredForNextLevel = GetXpRequiredForNextLevel();
        PlayerData.Instance.xpChanged.AddListener(XpChanged);
    }

    private void LevelUp() {
        currentLevel++;
        xpAtStartOfLevel = xpRequiredForNextLevel;
        xpRequiredForNextLevel = GetXpRequiredForNextLevel();
        playerLeveledUpEvent.Invoke();
    }

    private void XpChanged() {
        if (PlayerData.Instance.xp > xpRequiredForNextLevel) {
            LevelUp();
        }
    }

    public int GetPlayerCurrentLevel() {
        return (int)(Mathf.Sqrt(PlayerData.Instance.xp) * constant);
    }

    public int GetXpRequiredForNextLevel() {
        return Mathf.RoundToInt(Mathf.Pow(((currentLevel + 1) / constant), 2));
    }
}
