using Cinemachine;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour {

    private Rigidbody carRigidbody;
    
    [SerializeField]
    private float apparentSpeedMultiplier;

    private TextMeshProUGUI speedometer;
    private float textChangeCooldownTimer = 0.1f;
    
    // Start is called before the first frame update
    void Start() {
        speedometer = GetComponent<TextMeshProUGUI>();
        if (apparentSpeedMultiplier == 0.0f) {
            Debug.Log("For the speedometer to work properly, enter an Apparent Speed Multiplier value.");
        }
        if (PlayerData.Instance.playerVehicle != null) {
            PlayerVehiculeChanged(PlayerData.Instance.playerVehicle);
        }
    }

    private void OnEnable() {
        PlayerData.Instance.playerVehicleChanged.AddListener(PlayerVehiculeChanged);
    }

    private void OnDisable() {
        PlayerData.Instance?.playerVehicleChanged.RemoveListener(PlayerVehiculeChanged);
    }

    // Update is called once per frame
    void Update() {
        if (speedometer && carRigidbody) {
            if (textChangeCooldownTimer > 0) // small cooldown to avoid the text flickering too much
            {
                textChangeCooldownTimer -= Time.deltaTime;
                if (textChangeCooldownTimer > 0f)
                    return;

                textChangeCooldownTimer = 0.1f;
            }
            speedometer.text = ((int)(Mathf.Abs(carRigidbody.velocity.magnitude) * apparentSpeedMultiplier)).ToString() + " kph";
        }
    }

    private void PlayerVehiculeChanged(GameObject playerVehicle) {
        carRigidbody = playerVehicle.GetComponent<Rigidbody>();
    }
}
