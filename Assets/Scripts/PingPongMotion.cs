using UnityEngine;

public class PingPongMotion : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 0.5f;  // Try slower speeds first

    private Vector3 startPos;

    void Start()
    {
        // Ensure it starts at a known point
        startPos = transform.position;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(pointA, pointB, t);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the motion path in the scene view
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointA, pointB);
    }
}
