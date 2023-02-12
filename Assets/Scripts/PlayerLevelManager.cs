using UnityEngine;
using UnityEngine.Events;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerLevelManager : Singleton<PlayerLevelManager> {

    public UnityEvent playerLeveledUpEvent = new UnityEvent();

    [SerializeField]
    private float constant;

    public int currentLevel { get; private set; } = 0;
    public int xpAtStartOfLevel { get; private set; } = 0;
    public int xpRequiredForNextLevel { get; private set; } = int.MaxValue;

    private void Start() {
        xpRequiredForNextLevel = GetXpRequiredForLevel(currentLevel + 1);
        PlayerData.Instance.Xp.OnCurrentChanged.AddListener(XpChanged);
    }

    private void LevelUp() {
        currentLevel++;
        xpAtStartOfLevel = xpRequiredForNextLevel;
        xpRequiredForNextLevel = GetXpRequiredForLevel(currentLevel + 1);
        playerLeveledUpEvent.Invoke();
    }

    private void XpChanged(int currentXp) {
        if (currentXp > xpRequiredForNextLevel) {
            LevelUp();
        }
    }

    public int GetPlayerCurrentLevel() {
        return (int)(Mathf.Sqrt(PlayerData.Instance.Xp.Current) * constant);
    }

    public int GetXpRequiredForLevel(int level) {
        return Mathf.RoundToInt(Mathf.Pow((level / constant), 2));
    }
}
