using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour {

    [SerializeField]
    private float minimumSpeedForSmokeEffect;

    private ParticleSystem leftTireSmoke;
    private ParticleSystem rightTireSmoke;
    private TrailRenderer leftTireSkid;
    private TrailRenderer rightTireSkid;
    private Rigidbody carRigidbody;
    public float smokeTime = 0.0f;
    private bool smokeRequested = false;

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


        leftTireSkid = transform.Find("Effects").Find("LeftTireSkid").GetComponentInChildren<TrailRenderer>();
        rightTireSkid = transform.Find("Effects").Find("RightTireSkid").GetComponentInChildren<TrailRenderer>();

        if (leftTireSkid != null)
        {
            leftTireSkid.emitting = false;
        }
        if (rightTireSkid != null)
        {
            rightTireSkid.emitting = false;
        }
    }

    public void PlaySmoke()
    {
        smokeRequested = true;
        smokeTime = 0.0f;
    }

    // Update is called once per frame
    void Update() 
    {
        if (leftTireSmoke && rightTireSmoke)
        {
            //float carSpeed = Mathf.Abs(carRigidbody.velocity.magnitude);
            //if (carSpeed > minimumSpeedForSmokeEffect) {
            if (smokeRequested)
            {
                if (!leftTireSmoke.isPlaying)
                {
                    leftTireSmoke.Play();
                    rightTireSmoke.Play();
                }
            }
            else
            {
                if (!leftTireSmoke.isStopped)
                {
                    leftTireSmoke.Stop();
                    rightTireSmoke.Stop();
                }
            }
        }

        if (leftTireSkid && rightTireSkid)
        {
            if (smokeRequested)
            {
                if (!leftTireSkid.emitting)
                {
                    leftTireSkid.emitting = true;
                }
                if (!rightTireSkid.emitting)
                {
                    rightTireSkid.emitting = true;
                }
            }
            else
            {
                if (leftTireSkid.emitting)
                {
                    leftTireSkid.emitting = false;
                }
                if (rightTireSkid.emitting)
                {
                    rightTireSkid.emitting = false;
                }
            }
        }

        if (smokeRequested)
        {
            smokeTime += Time.deltaTime;
            if (smokeTime > 1.0f)
            {
                smokeRequested = false;
                smokeTime = 0.0f;
            }
        }
    }
}
