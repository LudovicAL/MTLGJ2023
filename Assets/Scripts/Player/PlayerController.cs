using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private List<AxleInfo> axleInfos; // the information about each individual axle

    [SerializeField]
    private float maxMotorTorque; // maximum torque the motor can apply to wheel

    [SerializeField]
    [Range(1.0f, 90.0f)]
    private float maxSteeringAngle; // maximum steer angle the wheel can have
    
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void FixedUpdate()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

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
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
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