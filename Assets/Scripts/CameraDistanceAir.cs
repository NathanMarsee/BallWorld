using UnityEngine;

public class CameraDistanceAir : MonoBehaviour
{
    public Transform target; // Set to PlayerRoot or Ball
    public Transform followBehind; // Set to GlobalPlaneGuide or PlaneGuide
    public float zoomInZ = -4f;
    public float zoomOutZ = -18f;
    public float zoomSpeed = 3f;
    public float holdThreshold = 0.3f;

    private float currentZ;
    private RotateToInputPlusCamera inputSource;
    private float forwardHold = 0f;
    private float backHold = 0f;

    void Start()
    {
        inputSource = followBehind.GetComponent<RotateToInputPlusCamera>();
        currentZ = transform.localPosition.z;
    }

    void Update()
    {
        Vector2 move = inputSource.move;

        if (move.y > 0.1f)
        {
            forwardHold += Time.deltaTime;
            backHold = 0;
        }
        else if (move.y < -0.1f)
        {
            backHold += Time.deltaTime;
            forwardHold = 0;
        }
        else
        {
            forwardHold = backHold = 0;
        }

        float targetZ = currentZ;
        if (forwardHold > holdThreshold) targetZ = zoomInZ;
        else if (backHold > holdThreshold) targetZ = zoomOutZ;

        currentZ = Mathf.Lerp(currentZ, targetZ, Time.deltaTime * zoomSpeed);

        // Position behind the player based on plane forward
        Vector3 offset = -followBehind.forward.normalized * Mathf.Abs(currentZ);
        transform.position = target.position + offset;
    }
}
