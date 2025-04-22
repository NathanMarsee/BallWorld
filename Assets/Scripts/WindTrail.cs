using UnityEngine;

public class WindTrail : MonoBehaviour
{
    public float sideOffset = 0.5f;            // Distance to the left/right from center
    public float verticalOffset = 0f;          // Optional Y offset
    public float maxTrailLength = 2f;          // Max stretch of the trail
    public float minSpeed = 2f;                // Trail shows after this speed

    private LineRenderer leftLine;
    private LineRenderer rightLine;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        leftLine = CreateTrailRenderer("LeftTrail");
        rightLine = CreateTrailRenderer("RightTrail");
    }

    LineRenderer CreateTrailRenderer(string name)
    {
        GameObject trail = new GameObject(name);
        trail.transform.parent = null; // Don't parent to ball so they don't rotate
        LineRenderer lr = trail.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.widthMultiplier = 0.05f;
        lr.numCapVertices = 3;
        lr.useWorldSpace = true;
        lr.startColor = new Color(1f, 1f, 1f, 0.5f);
        lr.endColor = new Color(1f, 1f, 1f, 0f);
        return lr;
    }

    void Update()
    {
        if (rb == null || rb.velocity.sqrMagnitude < 0.01f)
        {
            leftLine.enabled = false;
            rightLine.enabled = false;
            return;
        }

        float speed = rb.velocity.magnitude;
        Vector3 velocityDir = rb.velocity.normalized;

        // Get right vector perpendicular to velocity and up vector
        Vector3 up = Vector3.up;
        Vector3 right = Vector3.Cross(up, velocityDir).normalized;

        // Calculate trail source positions from ball center
        Vector3 leftPos = transform.position - right * sideOffset + Vector3.up * verticalOffset;
        Vector3 rightPos = transform.position + right * sideOffset + Vector3.up * verticalOffset;

        float trailLength = Mathf.Clamp(speed / 10f, 0.1f, maxTrailLength);
        Vector3 trailVector = -velocityDir * trailLength;

        // Enable and update trails
        leftLine.enabled = true;
        rightLine.enabled = true;

        leftLine.SetPosition(0, leftPos);
        leftLine.SetPosition(1, leftPos + trailVector);

        rightLine.SetPosition(0, rightPos);
        rightLine.SetPosition(1, rightPos + trailVector);
    }
}
