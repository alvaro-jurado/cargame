using System;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Wheel Configuration")]
    public GameObject tirePrefab;
    public TireAttributes[] tires;
    public float tireRadius = 0.53f;
    public float engineTorque = 450f;
    public float lateralGrip = 12f;
    public float maxLateralGrip = 12f;
    public float rollingResistance = 0.022f;

    [Header("Suspension System")]
    public float springForce = 90f;
    public float dampingCoefficient = 2.5f;
    public float maxSpringForce = 200f;

    [Header("Vehicle Mass")]
    public float vehicleMass = 100f;

    [Header("Audio")]
    public AudioSource engineAudioSource;
    public AudioClip engineSound;
    public AudioSource skidAudioSource;
    public AudioClip skidSound;

    [HideInInspector] public Vector2 inputVector = Vector2.zero;
    [HideInInspector] public bool isMovingForward = false;

    private Rigidbody vehicleRigidbody;
    private LayerMask surfaceLayerMask;

    void Start()
    {
        vehicleRigidbody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        surfaceLayerMask = LayerMask.GetMask("Asphalt", "Terrain");

        // Configurar sonidos
        if (engineAudioSource != null)
        {
            engineAudioSource.clip = engineSound;
            engineAudioSource.loop = true;
            engineAudioSource.Play();
        }

        foreach (var tire in tires)
        {
            tire.localAnchor = transform.InverseTransformPoint(transform.TransformPoint(tire.localAnchor));

            tire.tireVisual = Instantiate(tirePrefab, transform);
            tire.tireVisual.transform.localPosition = tire.localAnchor;
            tire.tireVisual.transform.localScale = 2f * new Vector3(tireRadius, tireRadius, tireRadius);

            tire.tireCircumference = 2f * Mathf.PI * tireRadius;
            tire.parentBody = vehicleRigidbody;
        }
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        float speed = vehicleRigidbody.velocity.magnitude;

        if (engineAudioSource != null)
        {
            engineAudioSource.pitch = Mathf.Lerp(0.8f, 2.0f, speed / 50f);
            engineAudioSource.volume = Mathf.Lerp(0.3f, 1.0f, speed / 50f);
        }

        bool isSkidding = false;

        foreach (var tire in tires)
        {
            if (!tire.tireVisual) continue;

            Transform tireTransform = tire.tireVisual.transform;
            Transform tireMesh = tireTransform.GetChild(0);

            if (tire.steeringType == 1)
            {
                float desiredAngle = tire.maxSteerAngle * inputVector.x;
                tireTransform.localRotation = Quaternion.Lerp(
                    tireTransform.localRotation,
                    Quaternion.Euler(0, desiredAngle, 0),
                    Time.fixedDeltaTime * 100f
                );
            }

            tire.worldTirePosition = transform.TransformPoint(tire.localAnchor);
            Vector3 velocityAtTire = vehicleRigidbody.GetPointVelocity(tire.worldTirePosition);
            tire.localVelocityVector = tireTransform.InverseTransformDirection(velocityAtTire);

            tire.appliedTorque = Mathf.Clamp(inputVector.y, -1f, 1f) * engineTorque / vehicleMass;
            float rollingResistanceForce = -rollingResistance * tire.localVelocityVector.z;
            float propulsionForce = tire.appliedTorque / tireRadius;

            float lateralFriction = -lateralGrip * tire.localVelocityVector.x;
            lateralFriction = Mathf.Clamp(lateralFriction, -maxLateralGrip, maxLateralGrip);

            if (Mathf.Abs(lateralFriction) > 4f)
            {
                isSkidding = true;
            }

            Vector3 localTotalForce = new Vector3(lateralFriction, 0f, rollingResistanceForce + propulsionForce);
            Vector3 worldTotalForce = tireTransform.TransformDirection(localTotalForce);

            isMovingForward = tire.localVelocityVector.z > 0f;

            if (Physics.Raycast(tire.worldTirePosition, -transform.up, out RaycastHit suspensionHit, tireRadius * 2f, surfaceLayerMask))
            {
                AdjustGripBySurface(suspensionHit.collider);

                float suspensionCompression = (tireRadius * 2f) - suspensionHit.distance;
                float dampingForce = (tire.previousSuspensionTravel - suspensionHit.distance) * dampingCoefficient;
                float suspensionTotalForce = (suspensionCompression + dampingForce) * springForce;
                suspensionTotalForce = Mathf.Clamp(suspensionTotalForce, 0f, maxSpringForce);

                Vector3 suspensionVector = suspensionHit.normal * suspensionTotalForce;
                vehicleRigidbody.AddForceAtPosition(suspensionVector + worldTotalForce, suspensionHit.point);

                tireTransform.position = suspensionHit.point + transform.up * tireRadius;
                tire.previousSuspensionTravel = suspensionHit.distance;
            }
            else
            {
                tireTransform.position = tire.worldTirePosition - transform.up * tireRadius;
            }

            Vector3 wheelMotionLocal = tireTransform.InverseTransformDirection(vehicleRigidbody.GetPointVelocity(tire.worldTirePosition));
            float tireRotationSpeed = wheelMotionLocal.z * 360f / tire.tireCircumference;
            tireMesh.Rotate(Vector3.right, tireRotationSpeed * Time.fixedDeltaTime, Space.Self);
        }

        if (skidAudioSource != null)
        {
            if (isSkidding)
            {
                if (!skidAudioSource.isPlaying)
                {
                    skidAudioSource.clip = skidSound;
                    skidAudioSource.loop = true;
                    skidAudioSource.Play();
                }
            }
            else
            {
                if (skidAudioSource.isPlaying)
                {
                    skidAudioSource.Stop();
                }
            }
        }
    }

    void AdjustGripBySurface(Collider hitCollider)
    {
        if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Asphalt"))
        {
            lateralGrip = 12f;
            rollingResistance = 0.022f;
        }
        else if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            lateralGrip = 2f;
            rollingResistance = 0.05f;
        }
    }
}
