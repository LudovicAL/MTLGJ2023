using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public int hp;
    public int xp = 0;
    public int zombieKilled = 0;

    public float murderSpeed = 10.0f;
    
    
    public static bool hasBullbar1 = false;
    public static bool hasBullbar2 = false;
    public static bool hasBullbar3 = false;
    public static bool hasBullbar5 = false;
    public static bool hasBullbar6 = false;
    public static bool hasEngine1 = false;
    public static bool hasExhaustPipe1 = false; //Raises car speed
    public static bool hasExhaustPipe1_M = false; //Raises car speed
    public static bool hasExhaustPipe2_1 = false; //Raises car speed
    public static bool hasExhaustPipe2_1_M = false; //Raises car speed
    public static bool hasExhaustPipe2_2 = false; //Raises car speed
    public static bool hasExhaustPipe2_2_M = false; //Raises car speed
    public static bool hasGasBalloon1 = false;  //Used for boosting
    public static bool hasHeadLight1_1 = false;
    public static bool hasHeadLight1_7 = false;
    public static bool hasHeadLight1_8 = false;
    public static bool hasHeadLight1_15 = false;
    public static bool hasHeadLight1_17 = false;
    public static bool hasJerrican1_1 = false;  //Used for boosting
    public static bool hasJerrican1_2 = false;  //Used for boosting
    public static bool hasProjector1 = false;
    public static bool hasProtection1_1 = false;
    public static bool hasProtection1_2 = false;
    public static bool hasProtection1_3 = false;
    public static bool hasProtection2_1 = false;
    public static bool hasProtection2_2 = false;
    public static bool hasProtection2_3 = false;
    public static bool hasProtection3_1 = false;
    public static bool hasProtection3_2 = false;
    public static bool hasRoofAttachments1 = false; //Raise car weight
    public static bool hasRoofAttachments2 = false; //Raise car weight
    public static bool hasRoofAttachments3 = false; //Raise car weight
    public static bool hasTire1_L = false;
    public static bool hasWeapon1_1 = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon1_2 = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon2_1 = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon2_1_M = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon2_2 = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon2_2_M = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon2_3 = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon2_3_M = false;    //Shoots bullets from the bullet reserve
    public static bool hasWeapon3 = false;    //Shoots bullets from the bullet reserve
    public static bool hasWinch1 = false;

    public static float boostReserve = 0.0f;
    public static int bulletsReserve = 0;
}
