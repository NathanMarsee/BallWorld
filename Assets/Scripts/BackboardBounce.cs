using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BackboardBounce : MonoBehaviour
{
    public float bounceForce = 5f;       // How strong the forward bounce is
    public float verticalBoost = 2f;     // Optional upward kick
    public AudioClip bounceSound;        // Assign your bounce sound in the Inspector
    public float volume = 1f;            // Volume control for the sound

    private AudioSource audioSource;

    private void Start()
    {
        // Create or get an AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            // Stop current motion
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Bounce slightly away from the surface normal
            Vector3 bounceDir = collision.contacts[0].normal * -1f;
            bounceDir.y += verticalBoost;

            rb.AddForce(bounceDir.normalized * bounceForce, ForceMode.VelocityChange);
        }

        // Play bounce sound if one is assigned
        if (bounceSound != null)
        {
            audioSource.PlayOneShot(bounceSound, volume);
        }
    }
}
