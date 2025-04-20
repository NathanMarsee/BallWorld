using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public Transform planeGuide;
    private Rigidbody rb;
    public float maxAgularVelocity;
    public float gravMag;
    public float killplane;
    public float slowDownRatio;
    public float minSpeedForMomentum = 15;
    public float maxSpeedForMomentum = 50;
    public int collisions;
    /*public float spinMag;
    private float inputIntensity;*/
    //private float offset;
    //private Vector2 move;
    private Vector3 gravDirection;
    private float lastVelocity;
    private float lastAngleVelocity;
    //private float baseMass;
    //private Vector3 spinDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAgularVelocity * 2f;
        //baseMass = rb.mass;

    if (planeGuide == null)
    {
        var foundGuide = GameObject.FindObjectOfType<RotateToInputPlusCamera>();
        if (foundGuide != null)
            planeGuide = foundGuide.transform;
        else
            Debug.LogWarning("BallControl: PlaneGuide not assigned and could not be found.");
    };
}


    // Update is called once per frame
    void FixedUpdate()
    {


        lastVelocity = rb.velocity.magnitude;
        Vector3 lastVelocityVector = rb.velocity;
        lastAngleVelocity = rb.angularVelocity.magnitude;
        //rb.angularVelocity = new Vector3(rb.angularVelocity.x * 0.99f, rb.angularVelocity.y * 0.9f, rb.angularVelocity.z * 0.99f);

        gravDirection = -planeGuide.up;
        Vector3 gravityForce = gravDirection * gravMag;
        if (collisions == 0)
            gravityForce = gravityForce * 0.8f;
        rb.AddForce(gravityForce);

        /*if (rb.velocity.magnitude < lastVelocity && collisions == 0)
            rb.velocity = rb.velocity + lastVelocityVector;
        else if (rb.velocity.magnitude < lastVelocity)
            rb.velocity = rb.velocity + (lastVelocityVector * ((Mathf.Clamp(lastVelocity, minSpeedForMomentum, maxSpeedForMomentum) - minSpeedForMomentum) / (maxSpeedForMomentum - minSpeedForMomentum)));
        */
        if (transform.position.y < killplane)
        {
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (rb.velocity.magnitude < 0.2)
        {
            rb.angularVelocity *= 0.98f;
        }
        if (rb.angularVelocity.magnitude * rb.angularVelocity.magnitude > Math.Sqrt(rb.velocity.magnitude / 2))
        {
            rb.angularVelocity *= 0.99f;
        }

        /*if (rb.velocity.magnitude > minSpeedForMass)
        {
            float extraMass = Mathf.Clamp(rb.velocity.magnitude, minSpeedForMass, maxSpeedForMass) - minSpeedForMass;
            extraMass = extraMass / (maxSpeedForMass - minSpeedForMass);
            rb.mass = baseMass + extraMass;
        } else
        {
            rb.mass = baseMass;
        }*/

    }
    void OnCollisionStay()
    {
        gravDirection = -planeGuide.up;
        rb.AddForce(gravDirection * gravMag * 0.5f);

        if (lastVelocity - rb.velocity.magnitude > slowDownRatio * 100 * lastVelocity)
        {
            rb.velocity *= 0.96f;
        }
        /*if (lastAngleVelocity - rb.angularVelocity.magnitude > slowDownRatio * 100 * lastAngleVelocity)
        {
            rb.angularVelocity *= 0.96f;
        }
        if (rb.angularVelocity.magnitude < 0.08f)
        {
            rb.angularVelocity = Vector3.zero;
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
    }
    private void OnCollisionExit(Collision collision)
    {
        collisions--;
    }
}
