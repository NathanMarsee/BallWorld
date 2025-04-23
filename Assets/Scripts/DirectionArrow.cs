using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Rigidbody targetRigidbody;
    public Camera cam;                   // Main Camera
    public Transform screenAnchor;      // Empty object at top-center of screen

    public float minSpeedThreshold = 0.1f;
    public float rotationSmoothSpeed = 10f;

    void LateUpdate()
    {
        if (targetRigidbody == null || cam == null || screenAnchor == null) return;

        // Always pin the arrow to the top-center of the screen
        transform.position = screenAnchor.position;

        Vector3 velocity = targetRigidbody.velocity;

        Quaternion targetRotation;

        if (velocity.magnitude > minSpeedThreshold)
        {
            // Rotate to match velocity direction in 3D space
            targetRotation = Quaternion.LookRotation(velocity.normalized, screenAnchor.up);
        }
        else
        {
            // If not moving, point "forward" in camera view
            targetRotation = screenAnchor.rotation;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
    }
}
