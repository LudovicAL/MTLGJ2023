using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour {

    [SerializeField]
    private float minimumSpeedForSmokeEffect;

    private ParticleSystem leftTireSmoke;
    private ParticleSystem rightTireSmoke;
    private Rigidbody carRigidbody;

    // Start is called before the first frame update
    void Start() {
        carRigidbody = GetComponent<Rigidbody>();
        leftTireSmoke = transform.Find("Effects").Find("LeftTireSmoke").GetComponentInChildren<ParticleSystem>();
        rightTireSmoke = transform.Find("Effects").Find("RightTireSmoke").GetComponentInChildren<ParticleSystem>();

        if (leftTireSmoke != null) {
            leftTireSmoke.Stop();
        }
        if (rightTireSmoke != null) {
            rightTireSmoke.Stop();
        }
    }

    // Update is called once per frame
    void Update() {
        if (leftTireSmoke && rightTireSmoke) {
            float carSpeed = Mathf.Abs(carRigidbody.velocity.magnitude);
            if (carSpeed > minimumSpeedForSmokeEffect) {
                if (!leftTireSmoke.isPlaying) {
                    leftTireSmoke.Play();
                    rightTireSmoke.Play();
                }
            } else {
                if (!leftTireSmoke.isStopped) {
                    leftTireSmoke.Stop();
                    rightTireSmoke.Stop();
                }
            }
        }
    }
}
