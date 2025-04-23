using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BackboardBounce : MonoBehaviour
{
    public float bounceForce = 5f;        // Multiplier for the bounce direction
    public float verticalBoost = 2f;      // Optional upward kick
    public float velocityDamping = 0.5f;  // Dampen how much of current velocity is kept (0 = stop, 1 = keep full)
    public AudioClip bounceSound;         // Assign bounce sound in Inspector
    public float volume = 1f;             // Volume control

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            // Slightly dampen current velocity
            rb.velocity *= velocityDamping;

            // Bounce away from surface normal with added upward force
            Vector3 bounceDir = collision.contacts[0].normal * -1f;
            bounceDir.y += verticalBoost;

            rb.AddForce(bounceDir.normalized * bounceForce, ForceMode.VelocityChange);
        }

        if (bounceSound != null)
            audioSource.PlayOneShot(bounceSound, volume);
    }
}
