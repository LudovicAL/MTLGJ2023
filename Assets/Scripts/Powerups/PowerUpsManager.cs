using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PowerUpsManager : Singleton<PowerUpsManager> {

    [SerializeField]
    private PowerUpSelectionMode powerUpSelectionMode;
    private List<PowerUp> availablePowerUps = new List<PowerUp>();

    void Start() {
        PlayerData.Instance.playerVehicleChanged.AddListener(UpdateAvailablePowerUpsList);
        UpdateAvailablePowerUpsList();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ActivatePowerUp(ScriptablePowerUp scriptablePowerUp) {
        int index = 0;
        int max = availablePowerUps.Count;
        for (; index < max; index++) {
            if (availablePowerUps[index].scriptablePowerUp == scriptablePowerUp) {
                break;
            }
        }
        if (index < max) {
            availablePowerUps[index].go.SetActive(true);
            availablePowerUps.RemoveAt(index);
        }
    }

    public List<ScriptablePowerUp> GetRandomPowerUps() {
        List<ScriptablePowerUp> scriptablePowerUps = new List<ScriptablePowerUp>();
        List<PowerUp> copyOfAvailablePowerUps = availablePowerUps.ToList();
        for (int i = 0; i < 3 && copyOfAvailablePowerUps.Count > 0; i++) {
            int randomIndex = Random.Range(0, copyOfAvailablePowerUps.Count);
            scriptablePowerUps.Add(copyOfAvailablePowerUps[randomIndex].scriptablePowerUp);
            copyOfAvailablePowerUps.RemoveAt(randomIndex);
        }
        return scriptablePowerUps;
    }

    public void UpdateAvailablePowerUpsList() {
        availablePowerUps = new List<PowerUp>();
        if (PlayerData.Instance.playerVehicle == null) {
            return;
        }
        ScriptablePowerUp[] scriptablePowerUps = Resources.LoadAll<ScriptablePowerUp>("ScriptablePowerUps");
        foreach (Transform childTransform in PlayerData.Instance.playerVehicle.transform.Find("PowerUps")) {
            foreach (ScriptablePowerUp spu in scriptablePowerUps) {
                if (spu.powerUpName == childTransform.name) {
                    availablePowerUps.Add(new PowerUp(spu, childTransform.gameObject));
                    break;
                }
            }
        }
    }

    private string GetAvailablePowerUpsAsString() {
        StringBuilder sb = new StringBuilder();
        for (int i = 0, max = availablePowerUps.Count; i < max; i++) {
            if (i > 0) {
                sb.Append(", ");
            }
            sb.Append(availablePowerUps[i].scriptablePowerUp.name);
        }
        return sb.ToString();
    }

    private class PowerUp {
        public ScriptablePowerUp scriptablePowerUp { get; private set; }
        public GameObject go { get; private set; }

        public PowerUp(ScriptablePowerUp scriptablePowerUp, GameObject go) {
            this.scriptablePowerUp = scriptablePowerUp;
            this.go = go; 
        }
    }

    private enum PowerUpSelectionMode {
        Sequential,
        Random
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PowerUpsManager))]
class PowerUpsManagerEditor : Editor {
    public override void OnInspectorGUI() {
        if (GUILayout.Button("GetPowerUpsOffer")) {
            PowerUpCanvas.instance.OfferPowerUps((PowerUpsManager)target);
        }
    }
}
#endif
