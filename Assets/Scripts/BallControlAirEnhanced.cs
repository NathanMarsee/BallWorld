using System;
using UnityEngine;

public class BallControlAirEnhanced : MonoBehaviour
{
    public Transform planeGuide;

    private Rigidbody rb;

    [Header("Physics Settings")]

    public float maxAgularVelocity = 20f; 
    // Controls how fast the ball can spin. Higher = faster spinning.

    public float gravMag = 9.8f; 
    // The base gravitational force pulling the ball down. Increase to fall faster.

    public float killplane = -10f; 
    // If the ball falls below this Y position, it will respawn. Lower = further fall before reset.

    public float slowDownRatio = 0.1f; 
    // Controls how much velocity is reduced on impact. Higher = bouncier, lower = snappier stop.

    public int collisions;
    // Tracks how many surfaces the ball is touching. Used to determine if airborne.

    private Vector3 gravDirection;
    private float lastVelocity;

    [Header("Air Control Tuning")]

    public float airSoarMultiplier = 0.1f; 
    // Reduces gravity when moving backward mid-air. Lower = floatier "soaring" effect.

    public float airPlummetMultiplier = 2.5f; 
    // Increases gravity when moving forward mid-air. Higher = faster plummeting.

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
        }
    }

    void FixedUpdate()
    {
        lastVelocity = rb.velocity.magnitude;
        gravDirection = -planeGuide.up;

        Vector3 gravityForce = gravDirection * gravMag;

        if (collisions == 0) // airborne
        {
            var inputSource = planeGuide.GetComponent<RotateToInputPlusCamera>();
            if (inputSource != null)
            {
                Vector2 moveInput = inputSource.move;

                if (moveInput.y < -0.1f)
                {
                    // Backwards input = soar
                    gravityForce *= airSoarMultiplier;
                }
                else if (moveInput.y > 0.1f)
                {
                    // Forwards input = plummet
                    gravityForce *= airPlummetMultiplier;
                }
            }

            gravityForce *= 0.8f; // Slightly reduced overall gravity while airborne
        }

        rb.AddForce(gravityForce);

        if (transform.position.y < killplane)
        {
            // Respawn ball if it falls below threshold
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SoundManager.Instance?.PlayLevelResetSound();
        }

        if (rb.velocity.magnitude < 0.2f)
        {
            // Slow down spin when nearly stationary
            rb.angularVelocity *= 0.98f;
        }

        if (rb.angularVelocity.sqrMagnitude > Mathf.Sqrt(rb.velocity.magnitude / 2))
        {
            // Prevent excessive spin if moving slowly
            rb.angularVelocity *= 0.99f;
        }
    }

    void OnCollisionStay()
    {
        // Apply extra gravity when grounded
        gravDirection = -planeGuide.up;
        rb.AddForce(gravDirection * gravMag * 0.5f);

        // Damp sudden slowdowns
        if (lastVelocity - rb.velocity.magnitude > slowDownRatio * 100 * lastVelocity)
        {
            rb.velocity *= 0.96f;
        }
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
