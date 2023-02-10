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

    private float currentMotorAmount = 0.0f;
    public void FixedUpdate()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

        currentMotorAmount = maxMotorTorque * inputVector.y;
        float steering = maxSteeringAngle * inputVector.x;
            
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = currentMotorAmount;
                axleInfo.rightWheel.motorTorque = currentMotorAmount;
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

    GUIStyle style = new GUIStyle();
    public void OnGUI()
    {
        style.fontSize = 50;
        style.normal.textColor = Color.red;

        string wheelDebugInfo = "MOTOR:  " + currentMotorAmount + "\n";
        foreach (AxleInfo axleInfo in axleInfos)
        {
            wheelDebugInfo += axleInfo.leftWheel.name + "\n";
            wheelDebugInfo += "RPM:  " + axleInfo.leftWheel.rpm + "\n";
            WheelHit hit = new WheelHit();
            if (axleInfo.leftWheel.GetGroundHit(out hit))
            {
                wheelDebugInfo += "Wheel Forward Slip:  " + hit.forwardSlip + "\n";
                wheelDebugInfo += "Wheel Side Slip:     " + hit.sidewaysSlip + "\n";
            }

        }

        GUI.Label(new Rect(10, 10, 100, 20), wheelDebugInfo, style);
    }
}
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}