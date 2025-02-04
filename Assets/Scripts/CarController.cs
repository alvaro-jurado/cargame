using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Transforms")]
    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    [Header("Car Settings")]
    public float motorTorque = 500f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 1000f;
    public float maxSpeed = 200f;
    public Rigidbody rb;

    [Header("UI")]
    public TextMeshProUGUI speedText;

    [Header("Anti-Flip Settings")]
    public float minSteerSpeed = 10f;
    public float maxSteerSpeed = 50f;
    public Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0);
    public float antiRollForce = 5000f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        ApplyAntiRollForce();
        UpdateWheelTransforms();
        UpdateSpeedUI();
    }

    private void HandleMotor()
    {
        float acceleration = Input.GetAxis("Vertical") * motorTorque;

        float maxSpeedMS = maxSpeed / 3.6f;

        if (rb.velocity.magnitude < maxSpeedMS)
        {
            rearLeftWheel.motorTorque = acceleration;
            rearRightWheel.motorTorque = acceleration;
        }
        else
        {
            rearLeftWheel.motorTorque = 0;
            rearRightWheel.motorTorque = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rearLeftWheel.brakeTorque = brakeTorque;
            rearRightWheel.brakeTorque = brakeTorque;
        }
        else
        {
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;
        }
    }

    private void HandleSteering()
    {
        float speed = rb.velocity.magnitude;
        float steerLimit = Mathf.Lerp(maxSteerAngle, maxSteerAngle * 0.3f, Mathf.InverseLerp(minSteerSpeed, maxSteerSpeed, speed));
        float steering = Input.GetAxis("Horizontal") * steerLimit;

        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;
    }

    private void ApplyAntiRollForce()
    {
        ApplyAntiRoll(frontLeftWheel, rearLeftWheel);
        ApplyAntiRoll(frontRightWheel, rearRightWheel);
    }

    private void ApplyAntiRoll(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float leftCompression = leftWheel.GetGroundHit(out hit) ? 1 - (hit.point.y - leftWheel.transform.position.y) : 1;
        float rightCompression = rightWheel.GetGroundHit(out hit) ? 1 - (hit.point.y - rightWheel.transform.position.y) : 1;

        float force = (leftCompression - rightCompression) * antiRollForce;
        if (leftWheel.isGrounded) rb.AddForceAtPosition(leftWheel.transform.up * -force, leftWheel.transform.position);
        if (rightWheel.isGrounded) rb.AddForceAtPosition(rightWheel.transform.up * force, rightWheel.transform.position);
    }

    private void UpdateWheelTransforms()
    {
        UpdateWheelTransform(frontLeftWheel, frontLeftTransform);
        UpdateWheelTransform(frontRightWheel, frontRightTransform);
        UpdateWheelTransform(rearLeftWheel, rearLeftTransform);
        UpdateWheelTransform(rearRightWheel, rearRightTransform);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void UpdateSpeedUI()
    {
        if (speedText != null)
        {
            float speedKmh = rb.velocity.magnitude * 3.6f;
            speedText.text = $"{Mathf.Round(speedKmh)} km/h";
        }
    }
}
