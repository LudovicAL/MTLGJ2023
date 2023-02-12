using UnityEngine;
using UnityEngine.InputSystem;

public class CarSounds : MonoBehaviour {

    [SerializeField]
    private float initialCarEnginePitch;
    [SerializeField]
    private float minimumSpeedForDriftingSounds;

    private AudioSource engineAudioSource;
    private AudioSource tireScreechAudioSource;
    private AudioSource hornAudioSource;
    private Rigidbody carRigidbody;

    //TODO: Hooking the two following values to something
    private bool isDrifting = false;
    private float carMaxSpeed = 100f;

    // Start is called before the first frame update
    void Start() {
        PlayerInputConveyer.Instance.playerInput.actions["Handbrake"].started += HandbrakeActivated;
        PlayerInputConveyer.Instance.playerInput.actions["Handbrake"].canceled += HandbrakeDeactivated;
        carRigidbody = GetComponent<Rigidbody>();
        hornAudioSource = transform.Find("AudioSource Horn").GetComponent<AudioSource>();
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

    private void Update() {
        if (Time.timeScale == 0) {
            if (engineAudioSource != null && engineAudioSource.isPlaying) {
                engineAudioSource.Stop();
            }
            if (tireScreechAudioSource != null && tireScreechAudioSource.isPlaying) {
                tireScreechAudioSource.Stop();
            }
            return;
        } else {
            if (engineAudioSource != null && !engineAudioSource.isPlaying) {
                engineAudioSource.Play();
            }
        }
    }

    public void BlowHorn(InputAction.CallbackContext context) {
        if (hornAudioSource && context.started) {
            if (hornAudioSource.isPlaying) {
                hornAudioSource.Stop();
            }
            hornAudioSource.Play();
        }
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

    private void HandbrakeActivated(InputAction.CallbackContext callbackContext) {
        isDrifting = true;
    }

    private void HandbrakeDeactivated(InputAction.CallbackContext callbackContext) {
        isDrifting = false;
    }
}
