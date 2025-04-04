using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public GameObject pos;
    public GameObject rotator;
    public GameObject planeGuide;
    //public GameObject cameraPlaneGuide;
    //public Vector2 rotTri;
    //public float distFromCenter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*transform.LookAt(target.transform.position);
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 3f, target.transform.position.z - 3.5f);*/

        /*if (Mathf.Abs(target.GetComponent<Rigidbody>().velocity.y) > Mathf.Abs(target.GetComponent<Rigidbody>().velocity.x) + Mathf.Abs(target.GetComponent<Rigidbody>().velocity.z))
        {
            transform.position = new Vector3(pos.transform.position.x, target.transform.position.y + 2, pos.transform.position.z);
            Vector3 targetDir = target.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime);
            transform.position = new Vector3(pos.transform.position.x, target.transform.position.y + 2, pos.transform.position.z);
            transform.LookAt(target.transform.position);
        }
        else*/
        {
            //*Mathf.Tan(transform.rotation.eulerAngles.x)

            //distFromCenter = Mathf.Sqrt(cameraPlaneGuide.transform.position.x * cameraPlaneGuide.transform.position.x + cameraPlaneGuide.transform.position.z * cameraPlaneGuide.transform.position.z);

            //rotTri = new Vector2(cameraPlaneGuide.transform.position.y, distFromCenter);

            //transform.position = new Vector3(pos.transform.position.x, target.transform.position.y + 0.7f + posDueToRot, pos.transform.position.z);
            transform.position = new Vector3(pos.transform.position.x, pos.transform.position.y, pos.transform.position.z);
            //transform.rotation = Quaternion.Euler(planeGuide.transform.rotation.eulerAngles.x, planeGuide.transform.rotation.eulerAngles.y + rotator.transform.rotation.eulerAngles.y, planeGuide.transform.rotation.eulerAngles.z);
            
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotator.transform.rotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            /*if (rotator.transform.rotation.eulerAngles.x >= 60)
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.x, 180);*/
        }
        //transform.LookAt(target.transform.position);
        //transform.rotation = Quaternion.Euler(pos.transform.rotation.x, pos.transform.rotation.y, pos.transform.rotation.z);
        //transform.rotation = Quaternion.Euler(30, rotator.transform.rotation.y, rotator.transform.rotation.z);
    }
}
