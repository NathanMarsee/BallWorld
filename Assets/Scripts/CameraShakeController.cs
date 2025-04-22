using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    [Header("Shake Settings")]
    public float speedThreshold = 5f;          // Speed where shake begins
    public float maxShakeMagnitude = 0.2f;     // Max shake offset
    public float shakeFrequency = 5f;          // How fast the shake moves (Hz)
    public float smoothTime = 0.3f;            // How smoothly shake intensity changes

    private Transform player;
    private Rigidbody playerRb;
    private Vector3 initialLocalPosition;

    private float targetShakeAmount = 0f;
    private float currentShakeAmount = 0f;
    private float shakeVelocity = 0f;

    void Start()
    {
        initialLocalPosition = transform.localPosition;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogWarning("CameraShakeController: No GameObject tagged Player found.");
        }
    }

    void LateUpdate()
    {
        if (playerRb == null) return;

        float speed = playerRb.velocity.magnitude;

        // Compute target shake amount using smooth ramp
        if (speed > speedThreshold)
        {
            float t = Mathf.InverseLerp(speedThreshold, speedThreshold * 3f, speed);
            targetShakeAmount = t * maxShakeMagnitude;
        }
        else
        {
            targetShakeAmount = 0f;
        }

        // Smoothly interpolate current shake
        currentShakeAmount = Mathf.SmoothDamp(currentShakeAmount, targetShakeAmount, ref shakeVelocity, smoothTime);

        // Apply shake offset
        float shakeX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2f - 1f;
        float shakeY = Mathf.PerlinNoise(0f, Time.time * shakeFrequency) * 2f - 1f;
        Vector3 shakeOffset = new Vector3(shakeX, shakeY, 0f) * currentShakeAmount;

        transform.localPosition = initialLocalPosition + shakeOffset;
    }
}
