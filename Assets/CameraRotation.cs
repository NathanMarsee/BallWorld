using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public GameObject planeGuide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(planeGuide.transform.rotation.eulerAngles.x, planeGuide.transform.rotation.eulerAngles.y, planeGuide.transform.rotation.eulerAngles.z);
    }
}
