using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteChunk : MonoBehaviour
{
    public float maxAngle;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1, 2) * maxAngle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
