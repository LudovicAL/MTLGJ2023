using UnityEngine;

public class ModelRotation : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    private float _previousTime;

    // Start is called before the first frame update
    void Start() {
        foreach (Transform t in transform) {
            t.gameObject.layer = LayerMask.NameToLayer("UI");
        }
    }

    // Update is called once per frame
    void Update() {
        
        transform.Rotate(Vector3.up * rotationSpeed * 0.008f);
    }
}
