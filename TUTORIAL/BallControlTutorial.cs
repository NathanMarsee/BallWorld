using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BallControlTutorial : MonoBehaviour
{
    public Transform planeGuide;
    private Rigidbody rb;
    public float maxAgularVelocity;
    public float gravMag;
    public float killplane;
    public float slowDownRatio;
    public int collisions;

    [Header("Sound Settings")]
    public float minVelocityForRollSound = 0.5f;
    public float collisionImpactThreshold = 2f;
    public float lowVelocityAngularThreshold = 0.1f;
    [SerializeField] private AudioSource rollingSoundSource; // Made serialized
    public AudioClip defaultRollSound;
    public float rollVolumeScaleFactor = 0.1f;
    public AudioClip collisionSound;

    private Vector3 gravDirection;
    private float lastVelocity;
    private bool isRolling = false;

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
                UnityEngine.Debug.LogWarning("BallControl: PlaneGuide not assigned and could not be found.");
        };

        // Ensure an AudioSource exists for rolling sounds if not assigned in Inspector
        if (rollingSoundSource == null)
        {
            GameObject rollingSoundObject = new GameObject("RollingSound");
            rollingSoundSource = rollingSoundObject.AddComponent<AudioSource>();
            rollingSoundSource.loop = true;
            rollingSoundSource.playOnAwake = false;
            rollingSoundSource.transform.SetParent(transform);
            rollingSoundSource.transform.localPosition = Vector3.zero;
        }

        if (defaultRollSound != null && rollingSoundSource.clip == null) // Only set if no clip is already assigned
        {
            rollingSoundSource.clip = defaultRollSound;
        }
    }

    void FixedUpdate()
    {
        lastVelocity = rb.velocity.magnitude;
        gravDirection = -planeGuide.up;
        Vector3 gravityForce = gravDirection * gravMag;
        if (collisions == 0)
            gravityForce = gravityForce * 0.8f;
        rb.AddForce(gravityForce);

        // Play/Stop rolling sound based on velocity
        if (rb.velocity.magnitude > minVelocityForRollSound && collisions > 0)
        {
            if (!isRolling)
            {
                rollingSoundSource.Play();
                isRolling = true;
            }
            rollingSoundSource.volume = Mathf.Clamp01(rb.velocity.magnitude * rollVolumeScaleFactor);
            rollingSoundSource.pitch = Mathf.Lerp(0.8f, 1.2f, Mathf.Clamp01(rb.velocity.magnitude / 5f)); // Adjust pitch based on speed
        }
        else
        {
            if (isRolling)
            {
                rollingSoundSource.Stop();
                isRolling = false;
            }
        }

        if (transform.position.y < killplane)
        {
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SoundManager.Instance?.PlayLevelResetSound();
            if (isRolling)
            {
                rollingSoundSource.Stop();
                isRolling = false;
            }
        }
        if (rb.velocity.magnitude < 0.2)
        {
            rb.angularVelocity *= 0.98f;
            if (rb.angularVelocity.magnitude < lowVelocityAngularThreshold && isRolling)
            {
                rollingSoundSource.Stop();
                isRolling = false;
            }
        }
        if (rb.angularVelocity.magnitude * rb.angularVelocity.magnitude > Math.Sqrt(rb.velocity.magnitude / 2))
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
        if (collisionSound != null && collision.relativeVelocity.magnitude > collisionImpactThreshold)
        {
            AudioSource.PlayClipAtPoint(collisionSound, collision.contacts[0].point, Mathf.Clamp01(collision.relativeVelocity.magnitude * 0.1f));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
        if (collisions <= 0 && rb.velocity.magnitude < minVelocityForRollSound)
        {
            rollingSoundSource.Stop();
            isRolling = false;
        }
    }
}