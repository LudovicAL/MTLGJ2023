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

    //[SerializeField]
    //private float maxMotorTorque; // maximum torque the motor can apply to wheel

    [SerializeField]
    [Range(1.0f, 90.0f)]
    private float maxSteeringAngle; // maximum steer angle the wheel can have
    
    private PlayerInput playerInput;

    private float wheelMassOverride = 50.0f;
    private float wheelStiffnessDefault = 1.0f;
    private float wheelSideFrictionAsymptoteOverride = 1.0f;
    private float frontWheelForwardFrictionOverride = 2.5f;
    private float rearWheelSideFrictionOverride = 1.5f;
    private float springOverride = 60000f;
    private float damperOverride = 6000f;
    private float brakeToReverseRPMThreshold = 100.0f;
    private float brakeStiffnessOverride = 5.0f;
    private float maxBrakeTorque = 3000;
    private float firstGearRPMThreshold = 500;
    private float firstGearStiffness = 5.0f;
    private float firstGearMotorTorque = 3500.0f;
    private float secondGearRPMThreshold = 600;
    private float secondGearMotorTorque = 2500.0f;
    private float thirdGearMotorTorque = 1500.0f;

    private float currentMotorTorque = 0.0f;
    private int currentGear = 0;
    private int gearState = 0; //too lazy for an enum, 1 is accelerate, 0 neutral, -1 is brake, -2 is reverse.
    private bool isHandbrake = false;
    private float handbrakeWheelStiffnessOverride = .75f;

    private float secretRocketBoostForce = 50.0f;

    public bool showDebugDisplay = true;

    private bool isSecretBoostOn = false;

    private void Start()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.mass = wheelMassOverride;
            axleInfo.rightWheel.mass = wheelMassOverride;

            WheelFrictionCurve forwardFrictionCurve = axleInfo.leftWheel.forwardFriction;
            if (axleInfo.wheelType == WheelType.FrontWheel)
            {
                forwardFrictionCurve.stiffness = frontWheelForwardFrictionOverride;
            }
            else if (axleInfo.wheelType == WheelType.RearWheel)
            {
                forwardFrictionCurve.stiffness = wheelStiffnessDefault;
            }

            axleInfo.leftWheel.forwardFriction = forwardFrictionCurve;
            axleInfo.rightWheel.forwardFriction = forwardFrictionCurve;


            WheelFrictionCurve sideFrictionCurve = axleInfo.leftWheel.sidewaysFriction;
            if (axleInfo.wheelType == WheelType.FrontWheel)
            {
                sideFrictionCurve.stiffness = wheelStiffnessDefault;
            }
            else if (axleInfo.wheelType == WheelType.RearWheel)
            {
                sideFrictionCurve.stiffness = rearWheelSideFrictionOverride;
            }
            sideFrictionCurve.asymptoteValue = wheelSideFrictionAsymptoteOverride;

            axleInfo.leftWheel.sidewaysFriction = sideFrictionCurve;
            axleInfo.rightWheel.sidewaysFriction = sideFrictionCurve;

            JointSpring suspensionSpring = axleInfo.leftWheel.suspensionSpring;
            suspensionSpring.spring = springOverride;
            suspensionSpring.damper = damperOverride;

            axleInfo.leftWheel.suspensionSpring = suspensionSpring;
            axleInfo.rightWheel.suspensionSpring = suspensionSpring;
        }
    }

    public void ActivateHandbrake(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            isHandbrake = true;

        if (callbackContext.canceled)
            isHandbrake = false;

        //Debug.Log(callbackContext);
    }

    private void ResetWheelStiffness(WheelCollider wheelCollider, bool isFrontWheel)
    {
        WheelFrictionCurve forwardFrictionCurve = wheelCollider.forwardFriction;
        if (isFrontWheel)
        {
            forwardFrictionCurve.stiffness = frontWheelForwardFrictionOverride;
        }
        else
        {
            forwardFrictionCurve.stiffness = wheelStiffnessDefault;
        }
        wheelCollider.forwardFriction = forwardFrictionCurve;

        WheelFrictionCurve sideFrictionCurve = wheelCollider.sidewaysFriction;
        if (isFrontWheel)
        {
            sideFrictionCurve.stiffness = 1.0f;
        }
        else
        {
            sideFrictionCurve.stiffness = 1.5f;
        }
        wheelCollider.sidewaysFriction = sideFrictionCurve;
    }

    private void UpdateCurrentGear(float averageRPM)
    {
        switch (gearState)
        {
            case 1:
                currentGear = 1;
                currentMotorTorque = firstGearMotorTorque;

                if (averageRPM > firstGearRPMThreshold)
                {
                    currentGear = 2;
                    currentMotorTorque = secondGearMotorTorque;
                }
                if (averageRPM > secondGearRPMThreshold)
                {
                    currentGear = 3;
                    currentMotorTorque = thirdGearMotorTorque;
                }

                break;
            case 0:
                currentGear = 0;
                currentMotorTorque = 0.0f;
                break;
            case -1:
                currentGear = 0;
                currentMotorTorque = 0.0f;
                break;
            case -2:
                currentGear = -1;
                currentMotorTorque = firstGearMotorTorque;
                break;
        }
    }

    private void UpdateWheelStiffnessForAcceleration(WheelCollider wheelCollider, bool isFrontWheel)
    {
        WheelFrictionCurve forwardFrictionCurve = wheelCollider.forwardFriction;
        if (wheelCollider.motorTorque != 0.0f)
        {
            /*
            switch(currentGear)
            {
                case 1:
                    forwardFrictionCurve.stiffness = firstGearStiffness;
                    break;
                case 2:
                    forwardFrictionCurve.stiffness = secondGearStiffness;
                    break;
                case 3:
                    forwardFrictionCurve.stiffness = thirdGearStiffness;
                    break;
            }
            */
            forwardFrictionCurve.stiffness = firstGearStiffness;
        }
        wheelCollider.forwardFriction = forwardFrictionCurve;
    }

    private void UpdateWheelStiffnessForBraking(WheelCollider wheelCollider, bool isFrontWheel)
    {
        WheelFrictionCurve forwardFrictionCurve = wheelCollider.forwardFriction;
        if (wheelCollider.brakeTorque != 0.0f)
        {
            forwardFrictionCurve.stiffness = brakeStiffnessOverride;
        }
        wheelCollider.forwardFriction = forwardFrictionCurve;
    }
    private void UpdateWheelStiffnessForHandbrake(WheelCollider wheelCollider)
    {
        WheelFrictionCurve sideFrictionCurve = wheelCollider.sidewaysFriction;
        if (wheelCollider.brakeTorque != 0.0f)
        {
            sideFrictionCurve.stiffness = handbrakeWheelStiffnessOverride;
        }
        wheelCollider.sidewaysFriction = sideFrictionCurve;
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private float currentMotorAmount = 0.0f;
    public void FixedUpdate()
    {
        gearState = 0;

        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

        currentMotorAmount = inputVector.y;

        float steering = maxSteeringAngle * inputVector.x;
        
        foreach (AxleInfo axleInfo in axleInfos) 
        {
            ResetWheelStiffness(axleInfo.leftWheel, axleInfo.wheelType == WheelType.FrontWheel);
            ResetWheelStiffness(axleInfo.rightWheel, axleInfo.wheelType == WheelType.FrontWheel);


            if (axleInfo.steering) 
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = 0.0f;
                axleInfo.rightWheel.motorTorque = 0.0f;

                if (currentMotorAmount > 0.0)
                {
                    if (axleInfo.leftWheel.rpm > -brakeToReverseRPMThreshold)
                    {
                        axleInfo.leftWheel.motorTorque = currentMotorAmount * currentMotorTorque;
                        gearState = 1;
                    }
                    if (axleInfo.rightWheel.rpm > -brakeToReverseRPMThreshold)
                    {
                        axleInfo.rightWheel.motorTorque = currentMotorAmount * currentMotorTorque;
                        gearState = 1;
                    }
                }
                else if (currentMotorAmount < 0.0)
                {
                    if (axleInfo.leftWheel.rpm < brakeToReverseRPMThreshold)
                    {
                        axleInfo.leftWheel.motorTorque = currentMotorAmount * currentMotorTorque;
                        gearState = -2;
                    }
                    if (axleInfo.rightWheel.rpm < brakeToReverseRPMThreshold)
                    {
                        axleInfo.rightWheel.motorTorque = currentMotorAmount * currentMotorTorque;
                        gearState = -2;
                    }
                }
            }

            if (axleInfo.brake)
            {
                axleInfo.leftWheel.brakeTorque = 0.0f;
                axleInfo.rightWheel.brakeTorque = 0.0f;

                if (currentMotorAmount > 0.0)
                {
                    if (axleInfo.leftWheel.rpm < -brakeToReverseRPMThreshold)
                    {
                        axleInfo.leftWheel.brakeTorque = currentMotorAmount * maxBrakeTorque;
                        gearState = -1;
                    }
                    if (axleInfo.rightWheel.rpm < -brakeToReverseRPMThreshold)
                    {
                        axleInfo.rightWheel.brakeTorque = currentMotorAmount * maxBrakeTorque;
                        gearState = -1;
                    }
                }
                else if (currentMotorAmount < 0.0)
                {
                    if (axleInfo.leftWheel.rpm > brakeToReverseRPMThreshold)
                    {
                        axleInfo.leftWheel.brakeTorque = currentMotorAmount * maxBrakeTorque * -1.0f;
                        gearState = -1;
                    }
                    if (axleInfo.rightWheel.rpm > brakeToReverseRPMThreshold)
                    {
                        axleInfo.rightWheel.brakeTorque = currentMotorAmount * maxBrakeTorque * -1.0f;
                        gearState = -1;
                    }
                }
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        float totalRPM = 0.0f;
        float wheelCount = 0;
        foreach (AxleInfo axleInfo in axleInfos)
        {
            totalRPM += axleInfo.leftWheel.rpm;
            totalRPM += axleInfo.rightWheel.rpm;
            wheelCount += 2;
        }
        UpdateCurrentGear(totalRPM / wheelCount);

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (gearState == -1)
            {
                UpdateWheelStiffnessForBraking(axleInfo.leftWheel, axleInfo.wheelType == WheelType.FrontWheel);
                UpdateWheelStiffnessForBraking(axleInfo.rightWheel, axleInfo.wheelType == WheelType.FrontWheel);
            }
            else if (gearState == 1)
            {
                UpdateWheelStiffnessForAcceleration(axleInfo.leftWheel, axleInfo.wheelType == WheelType.FrontWheel);
                UpdateWheelStiffnessForAcceleration(axleInfo.rightWheel, axleInfo.wheelType == WheelType.FrontWheel);
            }

            if (isHandbrake)
            {
                if (axleInfo.handbrake)
                {
                    axleInfo.leftWheel.brakeTorque = maxBrakeTorque;
                    axleInfo.rightWheel.brakeTorque = maxBrakeTorque;
                    UpdateWheelStiffnessForHandbrake(axleInfo.leftWheel);
                    UpdateWheelStiffnessForHandbrake(axleInfo.rightWheel);
                }
            }
        }

        isSecretBoostOn = false;
        if (gearState == 1)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            if (rb.velocity.magnitude < 10.0f)
            {
                isSecretBoostOn = true;
                rb.AddForce(transform.forward * secretRocketBoostForce, ForceMode.Acceleration);
            }
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

    void OnDrawGizmosSelected()
    {
        if (!showDebugDisplay)
        {
            return;
        }

        Rigidbody rb = transform.GetComponent<Rigidbody>();

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rb.centerOfMass + rb.position, 0.2f);
    }

    GUIStyle style = new GUIStyle();
    public void OnGUI()
    {
        if (!showDebugDisplay)
            return;

        style.fontSize = 25;
        style.normal.textColor = Color.red;

        string wheelDebugInfo = "GEARSTATE: " + gearState + "\n" + "GEAR: " + currentGear + "\n" + "MOTOR:  " + currentMotorAmount * currentMotorTorque + "\n";

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        wheelDebugInfo += "Velocity:  " + rb.velocity.magnitude + "\n";

        wheelDebugInfo += "SECRETBOOST:  ";
        if (isSecretBoostOn)
        {
            wheelDebugInfo += "ON";
        }
        else
        {
            wheelDebugInfo += "OFF";
        }
        wheelDebugInfo += "\n";

        foreach (AxleInfo axleInfo in axleInfos)
        {
            wheelDebugInfo += axleInfo.leftWheel.name + "\n";
            wheelDebugInfo += "RPM:  " + axleInfo.leftWheel.rpm + "\n";
            wheelDebugInfo += "MOTOR:  ";

            if (axleInfo.leftWheel.motorTorque != 0.0f)
                wheelDebugInfo += "ON \n";
            else
                wheelDebugInfo += "OFF \n";

            wheelDebugInfo += "BRAKE:  ";
            if (axleInfo.leftWheel.brakeTorque != 0.0f)
                wheelDebugInfo += "ON \n";
            else
                wheelDebugInfo += "OFF \n";

            wheelDebugInfo += "\n";



            wheelDebugInfo += axleInfo.rightWheel.name + "\n";
            wheelDebugInfo += "RPM:  " + axleInfo.rightWheel.rpm + "\n";
            wheelDebugInfo += "MOTOR:  ";

            if (axleInfo.rightWheel.motorTorque != 0.0f)
                wheelDebugInfo += "ON \n";
            else
                wheelDebugInfo += "OFF \n";

            wheelDebugInfo += "BRAKE:  ";
            if (axleInfo.rightWheel.brakeTorque != 0.0f)
                wheelDebugInfo += "ON \n";
            else
                wheelDebugInfo += "OFF \n";

            wheelDebugInfo += "\n";


        }

        foreach (AxleInfo axleInfo in axleInfos)
        {
            WheelHit hit = new WheelHit();
            if (axleInfo.leftWheel.GetGroundHit(out hit))
            {
                //wheelDebugInfo += "Wheel Forward Slip:  " + hit.forwardSlip + "\n";
                //wheelDebugInfo += "Wheel Side Slip:     " + hit.sidewaysSlip + "\n";
            }
        }


        GUI.Label(new Rect(10, 10, 100, 20), wheelDebugInfo, style);
    }
}
    
public enum WheelType
{
    FrontWheel,
    RearWheel
}

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
    public bool handbrake = false;
    public bool brake = false;
    public WheelType wheelType = WheelType.FrontWheel;
}