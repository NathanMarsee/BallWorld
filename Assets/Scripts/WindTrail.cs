using UnityEngine;

public class WindTrail : MonoBehaviour
{
    public Transform leftPoint;     // Assign TrailLeft
    public Transform rightPoint;    // Assign TrailRight
    public float maxTrailLength = 2f;   // Max stretch of the trail
    public float minSpeed = 2f;         // Trail shows after this speed

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
        trail.transform.parent = transform;
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
        if (rb == null) return;

        float speed = rb.velocity.magnitude;

        if (speed < minSpeed)
        {
            leftLine.enabled = false;
            rightLine.enabled = false;
            return;
        }

        leftLine.enabled = true;
        rightLine.enabled = true;

        Vector3 dir = -rb.velocity.normalized;
        float length = Mathf.Clamp(speed / 10f, 0.1f, maxTrailLength);

        leftLine.SetPosition(0, leftPoint.position);
        leftLine.SetPosition(1, leftPoint.position + dir * length);

        rightLine.SetPosition(0, rightPoint.position);
        rightLine.SetPosition(1, rightPoint.position + dir * length);
    }
}
