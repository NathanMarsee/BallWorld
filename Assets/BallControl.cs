using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BallControl : MonoBehaviour
{
    public Transform planeGuide;
    public GameObject scoreScreen;
    private Rigidbody rb;
    public float maxAgularVelocity;
    public float gravMag;
    public float killplane;
    public float slowDownRatio;
    public int collisions;
    public bool infiniteMode = false;
    public bool alive = true;
    public float rollGraceSeconds = 0.12f;

    [Header("Sound Settings")]
    public float minVelocityForRollSound = 0.5f;
    public float collisionImpactThreshold = 2f;
    public float lowVelocityAngularThreshold = 0.1f;
    [SerializeField] private AudioSource rollingSoundSource; // Made serialized
    [SerializeField] private AudioSource bonkSoundSource; // Made serialized
    public AudioClip defaultRollSound;
    public float rollVolumeScaleFactor = 0.1f;
    public AudioClip collisionSound;

    private Vector3 gravDirection;
    private float lastVelocity;
    private bool isRolling = false;
    private float timeSinceGround;

    private PlayerControls controls;
    private bool controlsFound = false;
    public bool restartActive = false;

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


    // Update is called once per frame
    void FixedUpdate()
    {
        if (restartActive)
        {
            if (controls.UI.Submit.ReadValue<float>() > 0.5f)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {

            if (!controlsFound)
            {
                controls = planeGuide.gameObject.GetComponent<RotateToInputPlusCamera>().controls;
                if (controls != null)
                    controlsFound = true;
            }

            if (collisions == 0)
            {
                timeSinceGround += Time.deltaTime;
            }

            lastVelocity = rb.velocity.magnitude;
            Vector3 lastVelocityVector = rb.velocity;
            //rb.angularVelocity = new Vector3(rb.angularVelocity.x * 0.99f, rb.angularVelocity.y * 0.9f, rb.angularVelocity.z * 0.99f);

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
                float volume = rb.velocity.magnitude / 20f;
                volume = Mathf.Clamp(volume, 0, 1f);
                rollingSoundSource.volume = volume;
                float pitch = rb.velocity.magnitude / 30f;
                if (pitch < 0.3)
                {
                    pitch = 0.3f;
                }
                rollingSoundSource.pitch = pitch; // Adjust pitch based on speed
            }
            else
            {
                if (isRolling && timeSinceGround > rollGraceSeconds)
                {
                    rollingSoundSource.Pause();
                    isRolling = false;
                }
            }

            if (infiniteMode)
                killplane = transform.position.z / -25 - 10;
            if (transform.position.y < killplane)
            {
                //SoundManager.Instance?.PlayLevelResetSound();
                StartCoroutine(Die());
            }
            if (rb.velocity.magnitude < 0.2)
            {
                rb.angularVelocity *= 0.98f;
                if (rb.angularVelocity.magnitude < lowVelocityAngularThreshold && isRolling && timeSinceGround > rollGraceSeconds)
                {
                    rollingSoundSource.Pause();
                    isRolling = false;
                }
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
        timeSinceGround = 0;
        if (collisionSound != null && lastVelocity > 1.3 * rb.velocity.magnitude)
        {
            bonkSoundSource.volume = Mathf.Clamp((lastVelocity - rb.velocity.magnitude) * 0.03f, 0, 0.5f);
            bonkSoundSource.pitch = Mathf.Clamp(lastVelocity / 15f, 0.8f, 3) * -0.3f + 1.5f;
            bonkSoundSource.Play();
            //AudioSource.PlayClipAtPoint(collisionSound, collision.contacts[0].point,);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        collisions--;
        if (collisions <= 0 && rb.velocity.magnitude < minVelocityForRollSound && timeSinceGround > rollGraceSeconds)
        {
            rollingSoundSource.Pause();
            isRolling = false;
        }
    }

    IEnumerator Die()
    {
        alive = false;
        yield return new WaitForSeconds(2);
        scoreScreen.SetActive(true);
        restartActive = true;
        rb.isKinematic = true;
        /*bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            rb.velocity = Vector3.zero;
            if (controls.UI.Submit.ReadValue<float>() > 0.5f)
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);*/
        /*transform.position = new Vector3(0, 0.5f, 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        alive = true;*/
    }
}
