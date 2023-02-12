using UnityEngine;

[CreateAssetMenu(fileName = "ScriptablePowerUp", menuName = "ScriptablePowerUp")]
public class ScriptablePowerUp : ScriptableObject {
    public string powerUpName;
    [Multiline(3)]
    public string powerUpDescription;
    public GameObject model;
    public float modelScaleMultiplier;
}