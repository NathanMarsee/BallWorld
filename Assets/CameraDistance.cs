using UnityEngine;

public class CameraDistance : MonoBehaviour
{
    [Header("References")]
    public GameObject cameraPlaneGuide;                      // Should be set to the plane guide controlling input rotation
    private RotateToInputPlusCamera inputSource;

    [Header("Zoom Settings")]
    public float defaultZ = -6f;
    public float zoomInZ = -4f;
    public float zoomOutZ = -8f;
    public float zoomSpeed = 3f;
    public float holdThreshold = 0.3f;

    private float forwardHoldTime = 0f;
    private float backwardHoldTime = 0f;
    private float currentZ;

    void Start()
    {
        if (cameraPlaneGuide == null)
        {
            Debug.LogWarning("CameraDistance: No cameraPlaneGuide assigned!");
            return;
        }

        inputSource = cameraPlaneGuide.GetComponent<RotateToInputPlusCamera>();
        if (inputSource == null)
        {
            Debug.LogWarning("CameraDistance: No RotateToInputPlusCamera found on guide!");
        }

        currentZ = defaultZ;
    }

    void Update()
    {
        if (cameraPlaneGuide == null || inputSource == null) return;

        // Handle hold timers
        Vector2 move = inputSource.move;

        if (move.y > 0.1f)
        {
            forwardHoldTime += Time.deltaTime;
            backwardHoldTime = 0f;
        }
        else if (move.y < -0.1f)
        {
            backwardHoldTime += Time.deltaTime;
            forwardHoldTime = 0f;
        }
        else
        {
            forwardHoldTime = 0f;
            backwardHoldTime = 0f;
        }

        float targetZ = defaultZ;

        if (forwardHoldTime >= holdThreshold)
            targetZ = zoomInZ;
        else if (backwardHoldTime >= holdThreshold)
            targetZ = zoomOutZ;

        // Smooth zoom transition
        currentZ = Mathf.Lerp(currentZ, targetZ, Time.deltaTime * zoomSpeed);

        // Apply camera position (X = stay fixed, Y = follow, Z = zoom distance)
        Vector3 newPos = new Vector3(transform.localPosition.x, cameraPlaneGuide.transform.position.y, currentZ);
        transform.localPosition = newPos;
    }
}
