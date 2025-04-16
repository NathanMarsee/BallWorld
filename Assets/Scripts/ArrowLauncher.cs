using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    public float power = 100f;
    public float upwardBoost = 50f;
    public Transform launchDirection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && launchDirection != null)
            {
                Vector3 force = launchDirection.forward.normalized * power +
                                Vector3.up * upwardBoost;

                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (launchDirection != null)
        {
            Gizmos.color = Color.red;
            Vector3 arcDirection = launchDirection.forward.normalized * power +
                                   Vector3.up * upwardBoost;
            Gizmos.DrawRay(launchDirection.position, arcDirection.normalized * 2f);
        }
    }
}
