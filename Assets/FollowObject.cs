using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
{
    if (target == null && GameObject.FindWithTag("Player") != null)
        target = GameObject.FindWithTag("Player");
}


    // Update is called once per frame
    void LateUpdate()
    {
        if (target.GetComponent<BallControl>().alive)
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
    }
}
