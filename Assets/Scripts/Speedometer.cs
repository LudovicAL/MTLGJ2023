using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour {

    [SerializeField]
    private Rigidbody carRigidbody;
    
    [SerializeField]
    private float apparentSpeedMultiplier;

    private TextMeshProUGUI speedometer;

    // Start is called before the first frame update
    void Start() {
        speedometer = GetComponent<TextMeshProUGUI>();
        if (apparentSpeedMultiplier == 0.0f) {
            Debug.Log("For the speedometer to work properly, enter an Apparent Speed Multiplier value.");
        }
    }

    // Update is called once per frame
    void Update() {
        if (speedometer && carRigidbody) {
            speedometer.text = ((int)(Mathf.Abs(carRigidbody.velocity.magnitude) * apparentSpeedMultiplier)).ToString() + " kph";
        }
    }
}
