using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float impactMultiplier = 0.5f;
    
    private Rigidbody _carRigidbody;
    private Rigidbody _destructibleRigidBody;
    
    private void Awake() => _destructibleRigidBody = GetComponent<Rigidbody>();

    private void OnEnable() => PlayerData.Instance.playerVehicleChanged.AddListener(Initialize);

    private void OnDisable()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.playerVehicleChanged.RemoveListener(Initialize);
        }
    } 

    private void Initialize(GameObject go) => _carRigidbody = go.GetComponent<Rigidbody>();


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Player"))
            return;

        Vector3 carVelocity = _carRigidbody.velocity;
        Vector3 force = new Vector3(carVelocity.x, Random.Range(5, 10), carVelocity.z);
        force *= impactMultiplier;
        _destructibleRigidBody.AddForceAtPosition(force, collision.GetContact(0).point, ForceMode.Impulse);
    }
}
