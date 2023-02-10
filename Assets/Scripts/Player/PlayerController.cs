using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque = 500.0f; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle = 50.0f; // maximum steer angle the wheel can have
    
    private Rigidbody _rigidBody;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable() => _playerInputActions.Player.Enable();
    private void OnDisable() => _playerInputActions.Player.Disable();
    public void FixedUpdate()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveVector = new Vector3(inputVector.x, 0, inputVector.y);
        
        float motor = maxMotorTorque * inputVector.y;
        float steering = maxSteeringAngle * inputVector.x;
            
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            //ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            //ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
    
    public void ApplyLocalPositionToVisuals(WheelCollider wheelCollider)
    {
        if (wheelCollider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = wheelCollider.transform.GetChild(0);

        wheelCollider.GetWorldPose(out var position, out var rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}