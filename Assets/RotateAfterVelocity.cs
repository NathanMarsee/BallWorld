using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAfterVelocity : MonoBehaviour
{
    public GameObject target;
    public Quaternion direction;
    private Rigidbody rb;
    private float rotSpeed;

    void Start()
    {
        if (target == null)
        {
            target = transform.parent?.gameObject;
            if (target == null)
            {
                Debug.LogWarning("RotateAfterVelocity: No target assigned and no parent found.");
                return;
            }
        }

        rb = target.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("RotateAfterVelocity: Target has no Rigidbody!");
        }
    }

    void LateUpdate()
    {
        if (rb == null || rb.velocity.sqrMagnitude < 0.001f)
            return;
        if (target.GetComponent<BallControl>().alive)
        {
            // Calculate rotation speed based on movement magnitude
            rotSpeed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z) / 1.5f;

            // Only rotate in the horizontal direction
            direction = Quaternion.LookRotation(rb.velocity);
            direction = Quaternion.Euler(0, direction.eulerAngles.y, 0);

            // Smoothly rotate toward movement direction
            transform.localRotation = Quaternion.Lerp(transform.localRotation, direction, Time.deltaTime * rotSpeed);
        } /*else
        {
            // Calculate rotation speed based on movement magnitude
            //rotSpeed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z) / 1.5f;

            // Rotate in the horizontal and vertical direction
            //direction = Quaternion.Euler((transform.position - target.transform.position).normalized);

            // Smoothly rotate toward movement direction
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, direction, Time.deltaTime * rotSpeed);

            Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position); transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);
            transform.rotation = lookOnLook;
        }*/
    }
}
