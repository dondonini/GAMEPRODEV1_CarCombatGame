using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo{

	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool drive;
	public bool steering;
}

public class CarController : MonoBehaviour
{

    public List<AxleInfo> axleList;
    public float maxTorque = 1000f;
    public float maxSpeed = 100f;
    public float maxBrake = 50f;
    public float maxSteeringAngle = 45f;
    public float antiRollValue = 5000f;
    public Rigidbody rigidBodyCar;
    public Transform newCentreOfMass;

    private float steerFactor = 0;

    public void Start(){
		Debug.Log(rigidBodyCar.centerOfMass);

        rigidBodyCar.centerOfMass = newCentreOfMass.localPosition;

    }

    public void FixedUpdate()
    {
        float motor = maxTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float brake = maxBrake * (1 - Mathf.Abs(Input.GetAxis("Vertical")));
        float currentSpeed = rigidBodyCar.velocity.magnitude;

        steerFactor = rigidBodyCar.velocity.magnitude * 3.6f / maxSpeed;

        foreach (AxleInfo axleInfo in axleList)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering * steerFactor;
                axleInfo.rightWheel.steerAngle = steering * steerFactor;
            }
            if (axleInfo.drive)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            axleInfo.leftWheel.brakeTorque = brake;
            axleInfo.rightWheel.brakeTorque = brake;

            AntiRoll(axleInfo.leftWheel, axleInfo.rightWheel);

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    private void AntiRoll(WheelCollider WheelL, WheelCollider WheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        bool groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * antiRollValue;

        if (groundedL)
            rigidBodyCar.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                   WheelL.transform.position);
        if (groundedR)
            rigidBodyCar.AddForceAtPosition(WheelR.transform.up * antiRollForce,
                   WheelR.transform.position);
    }

    private void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}

