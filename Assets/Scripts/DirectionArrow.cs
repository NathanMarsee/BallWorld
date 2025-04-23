using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [Header("Target Ball")]
    public Rigidbody targetRigidbody;

    [Header("Arrow Settings")]
    public float minSpeedThreshold = 0.1f;
    public float rotationSmoothSpeed = 15f;
    public Vector3 screenOffset = new Vector3(0f, 0.3f, 2f); // center-top of screen, 2 units forward

    [Header("Arrow Mesh")]
    public GameObject arrowMesh;
    public bool hideWhenStationary = true;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("DirectionArrow: No Main Camera found.");
        }
    }

    void LateUpdate()
    {
        if (targetRigidbody == null || mainCam == null) return;

        // Always stay in front of camera (top center)
        transform.position = mainCam.transform.position +
                             mainCam.transform.forward * screenOffset.z +
                             mainCam.transform.up * screenOffset.y +
                             mainCam.transform.right * screenOffset.x;

        Vector3 velocity = targetRigidbody.velocity;
        Vector3 direction = velocity.magnitude > minSpeedThreshold
            ? velocity.normalized
            : targetRigidbody.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);

        if (arrowMesh != null)
            arrowMesh.SetActive(!hideWhenStationary || velocity.magnitude > minSpeedThreshold);
    }
}
