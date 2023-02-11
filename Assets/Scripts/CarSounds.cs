using UnityEngine;

public class CarSounds : MonoBehaviour {

    [SerializeField]
    private float initialCarEnginePitch;
    [SerializeField]
    private float minimumSpeedForDriftingSounds;

    private AudioSource engineAudioSource;
    private AudioSource tireScreechAudioSource;
    private Rigidbody carRigidbody;

    private bool isDrifting = false;
    private float carMaxSpeed = 100f;

    // Start is called before the first frame update
    void Start() {
        carRigidbody = GetComponent<Rigidbody>();
        engineAudioSource = transform.Find("AudioSource Engine").GetComponent<AudioSource>();
        tireScreechAudioSource = transform.Find("AudioSource TireScreeching").GetComponent<AudioSource>();
        if (engineAudioSource) {
            engineAudioSource.gameObject.SetActive(true);
        }
        if (tireScreechAudioSource) {
            tireScreechAudioSource.gameObject.SetActive(true);
        }
        engineAudioSource.pitch = initialCarEnginePitch;
        InvokeRepeating("PlayCarSounds", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update() {

    }

    private void PlayCarSounds() {
        float carSpeed = Mathf.Abs(carRigidbody.velocity.magnitude);
        if (engineAudioSource != null) {
            engineAudioSource.pitch = Mathf.Clamp(initialCarEnginePitch + (carSpeed / 25f), 0.0f, 3.0f);
            engineAudioSource.volume = Mathf.Clamp01(carSpeed / (carMaxSpeed / 2.0f));
        }
        if (isDrifting && carSpeed > minimumSpeedForDriftingSounds) {
            if (!tireScreechAudioSource.isPlaying) {
                tireScreechAudioSource.Play();
            }
        } else if (!isDrifting || carSpeed < minimumSpeedForDriftingSounds) {
            if (tireScreechAudioSource.isPlaying) {
                tireScreechAudioSource.Stop();
            }
        }
    }
}
