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

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerLevelManager))]
public class PlayerLevelManagerEditor : Editor {
    public override void OnInspectorGUI() {
        var playerLevelManager = (PlayerLevelManager)target;
        if (playerLevelManager == null) {
            return;
        }

        if (GUILayout.Button("Log xp required for each levels")) {
            int XpRequiredForPreviousLevel = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0, max = 30; i < max; i++) {
                int XpRequiredForCurrentLevel = playerLevelManager.GetXpRequiredForLevel(i);
                sb.Append("Level " + i + ": " + XpRequiredForCurrentLevel
                    + "xp (Diff: " + (XpRequiredForCurrentLevel - XpRequiredForPreviousLevel) + ")\n");
                XpRequiredForPreviousLevel = XpRequiredForCurrentLevel;
            }
            Debug.Log(sb.ToString());
        }
        DrawDefaultInspector();
    }
}
#endif