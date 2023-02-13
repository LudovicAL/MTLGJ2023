using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : Singleton<VehicleSpawner> {
    public void SpawnVehicle(GameObject vehiclePrefab) {
        Instantiate(vehiclePrefab, transform.position, Quaternion.identity);
    }
}
