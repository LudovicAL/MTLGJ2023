using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "Data/PowerUpData")]
public class PowerUpData : ScriptableObject {
    public string powerUpName;
    [Multiline(3)]
    public string powerUpDescription;

    [Space(10)]
    public bool isPassive; 
    public bool isAcquired;
}