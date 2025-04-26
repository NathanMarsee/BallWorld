using System.Diagnostics;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{

    [Header("Rotation Settings")]
    [Tooltip("Speed of rotation around the specified axis in degrees per second.")]
    public float rotationSpeed = 30f;
    [Tooltip("Axis around which the object rotates.")]
    public Vector3 rotationAxis = Vector3.up; // Default to rotating around the Y axis

    [Header("Vertical Movement Settings")]
    [Tooltip("The object's vertical movement range, use 0 for no movement")]
    public float verticalRange = 2f;
    [Tooltip("Speed of vertical movement.")]
    public float moveSpeed = 1f;

    [Header("Audio Settings")]
    [Tooltip("The sound to play on collision.")]
    public AudioClip collisionSound;
    [Tooltip("The volume of the collision sound.")]
    [Range(0f, 1f)]
    public float collisionVolume = 1f;

    private Vector3 initialPosition;
    private float timeElapsed;
    private AudioSource audioSource; // Add an AudioSource

    void Start()
    {
        // Store the object's initial position
        initialPosition = transform.position;
        timeElapsed = 0f;

        // Add an AudioSource component to the GameObject
        audioSource = GetComponentInParent<AudioSource>();
    }

    void Update()
    {
        // Rotate the object around the specified axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);

        // Calculate vertical movement using a sine wave, centered around the initial Y position
        timeElapsed += Time.deltaTime;
        float verticalOffset = Mathf.Sin(timeElapsed * moveSpeed) * verticalRange;
        transform.position = new Vector3(transform.position.x, initialPosition.y + verticalOffset, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        // Play the collision sound
        if (collisionSound != null)
        {
            audioSource.PlayOneShot(collisionSound, collisionVolume);
        }

        // Destroy this object when it collides with another object
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}