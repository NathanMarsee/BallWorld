using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCam : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = transform.parent?.gameObject;
            if (target == null)
            {
                Debug.LogWarning("DeathCam: No target assigned and no parent found.");
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!target.GetComponent<BallControl>().alive)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position); transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);
            transform.rotation = lookOnLook;
        } else
        {
            transform.localRotation = Quaternion.Euler(25, 0, 0);
        }
    }
}
