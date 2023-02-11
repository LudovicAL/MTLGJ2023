using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour {

    [SerializeField]
    private Rigidbody carRigidbody;
    [SerializeField]
    private TextMeshProUGUI speedometer;
    [SerializeField]
    private float apparentSpeedMultiplier;

    // Start is called before the first frame update
    void Start() {
        if (speedometer == null) {
            GameObject go = GameObject.Find("Text Speed");
            if (go != null) {
                speedometer = go.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (speedometer) {
            speedometer.text = ((int)(Mathf.Abs(carRigidbody.velocity.magnitude) * apparentSpeedMultiplier)).ToString() + " kph";
        }
    }
}
