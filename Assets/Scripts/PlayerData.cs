using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : Singleton<PlayerData> {

    public int hp;

    public int zombieKilled = 0;
    public float murderSpeed = 10.0f;
    
    public bool hasBullbar1 = false;
    public bool hasBullbar2 = false;
    public bool hasBullbar3 = false;
    public bool hasBullbar5 = false;
    public bool hasBullbar6 = false;
    public bool hasEngine1 = false;
    public bool hasExhaustPipe1 = false; //Raises car speed
    public bool hasExhaustPipe1_M = false; //Raises car speed
    public bool hasExhaustPipe2_1 = false; //Raises car speed
    public bool hasExhaustPipe2_1_M = false; //Raises car speed
    public bool hasExhaustPipe2_2 = false; //Raises car speed
    public bool hasExhaustPipe2_2_M = false; //Raises car speed
    public bool hasGasBalloon1 = false;  //Used for boosting
    public bool hasHeadLight1_1 = false;
    public bool hasHeadLight1_7 = false;
    public bool hasHeadLight1_8 = false;
    public bool hasHeadLight1_15 = false;
    public bool hasHeadLight1_17 = false;
    public bool hasJerrican1_1 = false;  //Used for boosting
    public bool hasJerrican1_2 = false;  //Used for boosting
    public bool hasProjector1 = false;
    public bool hasProtection1_1 = false;
    public bool hasProtection1_2 = false;
    public bool hasProtection1_3 = false;
    public bool hasProtection2_1 = false;
    public bool hasProtection2_2 = false;
    public bool hasProtection2_3 = false;
    public bool hasProtection3_1 = false;
    public bool hasProtection3_2 = false;
    public bool hasRoofAttachments1 = false; //Raise car weight
    public bool hasRoofAttachments2 = false; //Raise car weight
    public bool hasRoofAttachments3 = false; //Raise car weight
    public bool hasTire1_L = false;
    public bool hasWeapon1_1 = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon1_2 = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon2_1 = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon2_1_M = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon2_2 = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon2_2_M = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon2_3 = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon2_3_M = false;    //Shoots bullets from the bullet reserve
    public bool hasWeapon3 = false;    //Shoots bullets from the bullet reserve
    public bool hasWinch1 = false;

    public float boostReserve = 0.0f;
    public int bulletsReserve = 0;

    //***************
    //XP
    //***************
    public int xp { get; private set; } = 0;
    public UnityEvent xpChanged = new UnityEvent();
    public void AddXp(int amountToAdd) {
        this.xp += amountToAdd;
        xpChanged.Invoke();
    }
    public void ResetXp() {
        this.xp = 0;
        xpChanged.Invoke();
    }

    //***************
    //PlayerVehicle
    //***************
    public GameObject playerVehicle { get; private set; }
    public UnityEvent playerVehicleChanged = new UnityEvent();
    public void SetPlayerVehicle(GameObject go) {
        this.playerVehicle = go;
        playerVehicleChanged.Invoke();
    }
}
