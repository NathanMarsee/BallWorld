using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteChunk : MonoBehaviour
{
    public float maxAngle;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1, 2) * maxAngle);
        player = FindObjectOfType<BallControl>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z - transform.position.z > 300)
            Destroy(gameObject);
    }
}
