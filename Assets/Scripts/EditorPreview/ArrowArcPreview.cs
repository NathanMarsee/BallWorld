using UnityEngine;

[ExecuteAlways]
public class ArrowArcPreview : MonoBehaviour
{
    [Header("Auto-Find Arrows By Tag")]
    public string arrowTag = "Arrow";

    [Header("Arc Settings")]
    public int resolution = 50;
    public float timeStep = 0.1f;

    private void OnDrawGizmos()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag(arrowTag);
        if (arrows.Length == 0) return;

        // Sort arrows based on Z (adjust axis as needed)
        System.Array.Sort(arrows, (a, b) => a.transform.position.z.CompareTo(b.transform.position.z));

        Vector3 totalForce = Vector3.zero;
        ArrowLauncher lastArrow = null;

        foreach (GameObject obj in arrows)
        {
            ArrowLauncher launcher = obj.GetComponent<ArrowLauncher>();
            if (launcher != null && launcher.launchDirection != null)
            {
                // Only add forward force, skip upwardBoost
                totalForce += launcher.launchDirection.forward.normalized * launcher.power;
                lastArrow = launcher;
            }
        }

        if (lastArrow == null || lastArrow.launchDirection == null)
            return;

        Vector3 startPos = lastArrow.launchDirection.position;
        Vector3 velocity = totalForce;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < resolution; i++)
        {
            Vector3 next = startPos + velocity * timeStep + 0.5f * Physics.gravity * timeStep * timeStep;
            Gizmos.DrawLine(startPos, next);

            velocity += Physics.gravity * timeStep;
            startPos = next;
        }
    }
}
