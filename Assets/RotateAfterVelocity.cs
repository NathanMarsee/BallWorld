using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAfterVelocity : MonoBehaviour
{
    public GameObject target;
    public Quaternion direction;
    private Rigidbody rb;
    private float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //rotations = transform.rotation;
        //Vector3 direction = Vector3.RotateTowards(transform.rotation.eulerAngles, rb.velocity, Time.deltaTime, 1.0f);
        /*if (target.GetComponent<Rigidbody>().velocity.magnitude > 0.05)
        {*/
        rotSpeed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z) / 1.5f;

        direction = Quaternion.LookRotation(rb.velocity);
        direction = Quaternion.Euler(0, direction.eulerAngles.y, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, direction, Time.deltaTime * rotSpeed);
        //}
        /*if (!(Mathf.Abs(rb.velocity.x) < 0.1) && !(Mathf.Abs(rb.velocity.z) < 0.1))
        {
            Quaternion direction = Quaternion.LookRotation(rb.velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, Time.deltaTime * 2);
        }*/
    }
}
