using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleRegister : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerData.Instance.SetPlayerVehicle(gameObject);
        Destroy(this);
    }
}
