using UnityEngine;

public class CarSounds : MonoBehaviour {

    [SerializeField]
    private bool useSounds = false;
    [SerializeField]
    private AudioSource carEngineSound;
    [SerializeField]
    private AudioSource tireScreechSound;
    [SerializeField]
    private float initialCarEnginePitch;
    [SerializeField]
    private AudioSource engineAudioSource;
    [SerializeField]
    private AudioSource tireScreechAudioSource;
    [SerializeField]
    private Rigidbody carRigidbody;

    private bool isDrifting = false;
    private float carMaxSpeed = 100f;

    // Start is called before the first frame update
    void Start() {
        engineAudioSource.pitch = initialCarEnginePitch;
        InvokeRepeating("PlayCarSounds", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update() {

    }

    private void PlayCarSounds() {
        if (useSounds) {
            float carSpeed = Mathf.Abs(carRigidbody.velocity.magnitude);
            if (carEngineSound != null) {
                carEngineSound.pitch = Mathf.Clamp(initialCarEnginePitch + (carSpeed / 25f), 0.0f, 3.0f);
                carEngineSound.volume = Mathf.Clamp01(carSpeed / (carMaxSpeed / 2.0f));
            }
            if (isDrifting || (carSpeed > 12f)) {
                if (!tireScreechSound.isPlaying) {
                    tireScreechSound.Play();
                }
            } else if ((!isDrifting) && (carSpeed < 12f)) {
                tireScreechSound.Stop();
            }
        } else if (!useSounds) {
            if (carEngineSound != null && carEngineSound.isPlaying) {
                carEngineSound.Stop();
            }
            if (tireScreechSound != null && tireScreechSound.isPlaying) {
                tireScreechSound.Stop();
            }
        }
    }
}
