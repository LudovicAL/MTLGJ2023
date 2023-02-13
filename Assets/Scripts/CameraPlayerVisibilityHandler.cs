using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerVisibilityHandler : MonoBehaviour
{
    class BuildingInfo
    {
        public GameObject building;
        public Material previousMaterial;
        public bool hitThisFrame;
    }

    [SerializeField] Material hiddenMaterial;
    
    private GameObject player;
    private Camera cam;

    private int layerMask;
    private RaycastHit[] hitResults = new RaycastHit[5];

    private Dictionary<int, BuildingInfo> hiddenBuildings = new Dictionary<int, BuildingInfo>();
    private List<int> toRemoveFromHidden = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        layerMask = 1 << 7;
    }

    private void OnEnable() {
        PlayerData.Instance.playerVehicleChanged.AddListener(PlayerVehicleChanged);
    }

    private void OnDisable() {
        PlayerData.Instance.playerVehicleChanged.RemoveListener(PlayerVehicleChanged);
    }

    private void PlayerVehicleChanged(GameObject newVehicle) {
        player = newVehicle;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.Instance.currentState != GameState.Started) {
            return;
        }
        Vector3 cameraPos = cam.transform.position;
        Vector3 playerPos = player.transform.position;
        Vector3 playerToCamera = cameraPos - playerPos;
        Vector3 playerToCameraNormalized = playerToCamera.normalized;

        foreach (BuildingInfo info in hiddenBuildings.Values)
            info.hitThisFrame = false;
        
        int size = Physics.RaycastNonAlloc(playerPos, playerToCameraNormalized, hitResults, playerToCamera.magnitude, layerMask);
        for (int i = 0; i < size; i++)
        {
            RaycastHit hit = hitResults[i];
            GameObject hitObject = hit.transform.gameObject;
            int objectInstanceID = hitObject.GetInstanceID();
            
            if (!hiddenBuildings.ContainsKey(objectInstanceID))
            {
                MeshRenderer buildingRenderer = hitObject.GetComponent<MeshRenderer>();
                if (buildingRenderer == null) continue;

                hiddenBuildings.Add(objectInstanceID, new BuildingInfo
                {
                    building = hitObject,
                    previousMaterial = buildingRenderer.material,
                    hitThisFrame = true
                });

                // change material to hidden
                buildingRenderer.material = hiddenMaterial;
            }
            else
            {
                hiddenBuildings[objectInstanceID].hitThisFrame = true;
            }
        }

        toRemoveFromHidden.Clear();
        foreach (int key in hiddenBuildings.Keys)
        {
            BuildingInfo info = hiddenBuildings[key];
            if (info.hitThisFrame)
                continue;

            info.building.GetComponent<MeshRenderer>().material = info.previousMaterial;
            
            toRemoveFromHidden.Add(key);
        }

        foreach (int key in toRemoveFromHidden)
            hiddenBuildings.Remove(key);
    }
}
