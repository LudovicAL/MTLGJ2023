using UnityEngine;
using Random = UnityEngine.Random;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float impactMultiplier = 100;
    
    private GameObject playerObject;
    private Rigidbody carRigidbody;
    private Rigidbody destructibleRigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        playerObject = FindObjectOfType<PlayerController>().gameObject;
        carRigidbody = playerObject.GetComponent<Rigidbody>();
        destructibleRigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject != playerObject)
            return;

        Vector3 carVelocity = carRigidbody.velocity;
        Vector3 force = new Vector3(carVelocity.x, Random.Range(5, 10), carVelocity.z);
        destructibleRigidBody.AddForceAtPosition(force, other.GetContact(0).point, ForceMode.Impulse);
    }
}
