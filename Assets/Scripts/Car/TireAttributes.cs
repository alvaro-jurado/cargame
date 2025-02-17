using System;
using UnityEngine;

[Serializable]
public class TireAttributes
{
    public int steeringType = 1;    // 1 = steerable, 0 = free rotation
    public Vector3 localAnchor;     // Wheel anchor in local car space
    public float maxSteerAngle = 30f;   // Maximum steering angle

    // Hidden simulation variables
    [HideInInspector] public float previousSuspensionTravel = 0.0f;
    [HideInInspector] public Vector3 localSlipVector;
    [HideInInspector] public Vector3 worldSlipVector;
    [HideInInspector] public Vector3 suspensionForceVector;
    [HideInInspector] public Vector3 worldTirePosition;
    [HideInInspector] public float tireCircumference;
    [HideInInspector] public float appliedTorque = 0.0f;
    [HideInInspector] public Rigidbody parentBody;
    [HideInInspector] public GameObject tireVisual;
    [HideInInspector] public float contactForce;
    [HideInInspector] public Vector3 localVelocityVector;
}