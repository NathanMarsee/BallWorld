using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistance : MonoBehaviour
{
    public GameObject cameraPlaneGuide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float parentRot = GetComponentInParent<Transform>().rotation.eulerAngles.y;
        parentRot *= Mathf.PI / 180;
        //parentRot += Mathf.PI;
        transform.localPosition = new Vector3(transform.localPosition.x, cameraPlaneGuide.transform.position.y, cameraPlaneGuide.transform.position.z);
    }
}
