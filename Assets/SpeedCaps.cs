using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCaps : MonoBehaviour
{
    private Rigidbody rb;
    public float maxAgularVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAgularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
