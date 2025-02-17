using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeedX = 250.0f;
    public float rotationSpeedY = 120.0f;
    public float minYAngle = -20f;
    public float maxYAngle = 80f;

    private float currentX = 0.0f;
    private float currentY = 20.0f;
    private bool isMovingCamera = false;
    private Camera cam;

    private int cameraIndex = 0;
    private readonly float[] fovs = { 50f, 70f, 90f };

    private Vector3[] cameraPositions;
    private Vector3[] cameraRotations;

    void Start()
    {
        if (target == null)
        {
            enabled = false;
            return;
        }

        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraController requires a Camera component on the same GameObject.");
            enabled = false;
            return;
        }

        cameraPositions = new Vector3[]
        {
            new Vector3(0, 2.5f, -6f), // 3rd per (50 FOV)
            new Vector3(0, 2.5f, -7f), // 3rd per (70 FOV)
            new Vector3(0, 2.5f, -8f), // 3rd per (90 FOV)
            new Vector3(-1.86f, 0.5f, 0.5f), // Wheel cam
            new Vector3(0, 1.2f, 1.5f), // Bonnet cam
            new Vector3(-0.2f, 1f, -0.1f)  // 1st per
        };

        cameraRotations = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(5, 0, 0),
            new Vector3(-1, 0, 0)
        };

        cam.fieldOfView = fovs[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraIndex = (cameraIndex + 1) % cameraPositions.Length;
            if (cameraIndex < 3)
            {
                cam.fieldOfView = fovs[cameraIndex];
            }
        }

        if (cameraIndex < 3)
        {
            if (Input.GetMouseButton(1))
            {
                isMovingCamera = true;
                currentX += Input.GetAxis("Mouse X") * rotationSpeedX * Time.deltaTime;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeedY * Time.deltaTime;
                currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
            }
            else if (isMovingCamera)
            {
                isMovingCamera = false;
                currentX = 0;
                currentY = 20;
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + target.TransformDirection(cameraPositions[cameraIndex]);
        Quaternion rotation = Quaternion.Euler(cameraRotations[cameraIndex] + new Vector3(currentY, currentX, 0));
        transform.rotation = target.rotation * rotation;
    }
}
