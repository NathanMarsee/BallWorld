using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public Vector3 centerPoint = Vector3.zero; // The point to orbit around
    public float radius = 5f;                  // Distance from center
    public float speed = 1f;                   // How fast it orbits
    public Vector3 axis = Vector3.up;          // Axis of rotation (Y=horizontal circle)

    private float angle;

    void Update()
    {
        angle += speed * Time.deltaTime;
        Vector3 offset = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis) * (Vector3.forward * radius);
        transform.position = centerPoint + offset;
    }
}
