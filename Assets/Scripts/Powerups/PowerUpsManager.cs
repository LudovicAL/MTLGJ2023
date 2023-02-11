using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour {
    // Start is called before the first frame update

    private List<AvailablePowerUp> availablePowerUps = new List<AvailablePowerUp>();

    void Start() {
        availablePowerUps = GetAvailablePowerUps();
        Debug.Log("The car has " + availablePowerUps.Count + " available powerups: " + GetAvailablePowerUpsAsString());
    }

    // Update is called once per frame
    void Update() {
        
    }

    private List<AvailablePowerUp> GetAvailablePowerUps() {
        List<AvailablePowerUp> result = new List<AvailablePowerUp>();
        ScriptablePowerUp[] scriptablePowerUps = Resources.LoadAll<ScriptablePowerUp>("ScriptablePowerUps");
        foreach (Transform childTransform in transform) {
            foreach (ScriptablePowerUp spu in scriptablePowerUps) {
                if (spu.powerUpName == childTransform.name) {
                    result.Add(new AvailablePowerUp(spu, childTransform.gameObject));
                    break;
                }
            }
        }
        return result;
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

    private class AvailablePowerUp {
        public ScriptablePowerUp scriptablePowerUp { get; private set; }
        public GameObject go { get; private set; }

        public AvailablePowerUp(ScriptablePowerUp scriptablePowerUp, GameObject go) {
            this.scriptablePowerUp = scriptablePowerUp;
            this.go = go; 
        }
    }
}
