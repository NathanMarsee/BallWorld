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
    /*public float spinMag;
    private float inputIntensity;*/
    //private float offset;
    //private Vector2 move;
    private Vector3 gravDirection;
    private float lastVelocity;
    private float lastAngleVelocity;
    //private Vector3 spinDirection;
    // Start is called before the first frame update
    void Start()
{
    rb = GetComponent<Rigidbody>();
    rb.maxAngularVelocity = maxAgularVelocity * 2f;

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
        gravDirection = -planeGuide.up;
        rb.AddForce(gravDirection * gravMag);
        
        lastVelocity = rb.velocity.magnitude;
        
        lastAngleVelocity = rb.angularVelocity.magnitude;

        if(transform.position.y < killplane)
        {
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    void OnCollisionStay()
    {
        gravDirection = -planeGuide.up;
        rb.AddForce(gravDirection * gravMag * 0.5f);

        if (lastVelocity - rb.velocity.magnitude > slowDownRatio * 100 * lastVelocity)
        {
            rb.velocity *= 0.96f;
        }
        if (rb.velocity.magnitude < 0.08f)
        {
            rb.velocity = Vector3.zero;
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
}
