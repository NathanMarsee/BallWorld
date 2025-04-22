using System;
using UnityEngine;

public class BallControlAirEnhanced : MonoBehaviour
{
    public Transform planeGuide;
    private Rigidbody rb;

    [Header("Physics Settings")]
    public float maxAgularVelocity = 20f;
    public float gravMag = 9.8f;
    public float killplane = -140f;
    public float slowDownRatio = 5f;
    public int collisions;

    private Vector3 gravDirection;
    private float lastVelocity;

    [Header("Air Control Tuning")]
    public float airSoarMultiplier = 0.9f;
    public float airPlummetMultiplier = 80f;
    public float airStrafeForce = 30f;
    public float airForwardBoost = 30f;
    public float airBackwardFloat = 10f;

    [Header("Air Control Acceleration")]
    public float controlAcceleration = 60f;
    public float controlDecay = 80f;

    private float currentForwardForce = 0f;
    private float currentBackwardForce = 0f;
    private float currentStrafeForce = 0f;

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

                float velocityScale = Mathf.Clamp01(rb.velocity.magnitude / 10f); // More speed = more control

                // --- Forward soar ---
                if (moveInput.y > 0.1f)
                {
                    gravityForce *= airSoarMultiplier;
                    currentForwardForce = Mathf.MoveTowards(
                        currentForwardForce,
                        moveInput.y * airForwardBoost * velocityScale,
                        controlAcceleration * Time.fixedDeltaTime
                    );
                }
                else
                {
                    currentForwardForce = Mathf.MoveTowards(currentForwardForce, 0f, controlDecay * Time.fixedDeltaTime);
                }

                // --- Backward dive ---
                if (moveInput.y < -0.1f)
                {
                    gravityForce *= airPlummetMultiplier;
                    currentBackwardForce = Mathf.MoveTowards(
                        currentBackwardForce,
                        Mathf.Abs(moveInput.y) * airBackwardFloat * velocityScale,
                        controlAcceleration * Time.fixedDeltaTime
                    );
                }
                else
                {
                    currentBackwardForce = Mathf.MoveTowards(currentBackwardForce, 0f, controlDecay * Time.fixedDeltaTime);
                }

                // --- Strafe ---
                if (Mathf.Abs(moveInput.x) > 0.1f)
                {
                    float targetStrafe = moveInput.x * airStrafeForce * velocityScale;
                    currentStrafeForce = Mathf.MoveTowards(currentStrafeForce, targetStrafe, controlAcceleration * Time.fixedDeltaTime);
                }
                else
                {
                    currentStrafeForce = Mathf.MoveTowards(currentStrafeForce, 0f, controlDecay * Time.fixedDeltaTime);
                }

                // Apply smoothed air forces
                Vector3 forwardDir = planeGuide.forward;
                Vector3 rightDir = planeGuide.right;

                rb.AddForce(forwardDir * currentForwardForce, ForceMode.Acceleration);
                rb.AddForce(-forwardDir * currentBackwardForce, ForceMode.Acceleration);
                rb.AddForce(rightDir * currentStrafeForce, ForceMode.Acceleration);

                // 🔻 Velocity damping
                Vector3 flatVel = rb.velocity;
                flatVel.y = 0;

                float forwardSpeed = Vector3.Dot(flatVel, forwardDir);
                float strafeSpeed = Vector3.Dot(flatVel, rightDir);

                Vector3 forwardDecay = -forwardDir * forwardSpeed * 0.05f;
                Vector3 strafeDecay = -rightDir * strafeSpeed * 0.05f;

                rb.AddForce(forwardDecay, ForceMode.Acceleration);
                rb.AddForce(strafeDecay, ForceMode.Acceleration);
            }

            gravityForce *= 0.8f;
        }

        rb.AddForce(gravityForce);

        if (transform.position.y < killplane)
        {
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SoundManager.Instance?.PlayLevelResetSound();
        }

        if (rb.velocity.magnitude < 0.2f)
        {
            rb.angularVelocity *= 0.98f;
        }

        if (rb.angularVelocity.sqrMagnitude > Mathf.Sqrt(rb.velocity.magnitude / 2))
        {
            rb.angularVelocity *= 0.99f;
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
