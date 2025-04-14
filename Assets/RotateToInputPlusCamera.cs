using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateToInputPlusCamera : MonoBehaviour
{
    public GameObject cam;
    public float offset;
    public Vector2 move;
    private Quaternion rotateBy;
    private bool newDeltaObtained;
    public float totalAngle;
    public Quaternion thisAngle;
    public float inputAngle;
    public float finalInputAngle;
    public float inputIntensity;
    // Start is called before the first frame update
    void Start()
    {

    }
    void FixedUpdate()
    {
        thisAngle = transform.rotation;
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        move = gamepad.leftStick.ReadValue();
        /*if (move.y > 0)
            move.y *= 0.5f;*/
        inputIntensity = (move.x * move.x + move.y * move.y);

        offset = 90 - cam.transform.rotation.eulerAngles.y;

        while (offset < 0)
        {
            offset += 360;
        }

        inputAngle = Mathf.Atan2(-move.y, -move.x);
        inputAngle *= 180 / Mathf.PI;
        while (inputAngle < 0)
        {
            inputAngle += 360;
        }

        finalInputAngle = (inputAngle + offset);
        float finalInputAngleRad = finalInputAngle * Mathf.PI / 180;
        float finalInputX = Mathf.Pow(inputIntensity, 1.1f) * Mathf.Cos(finalInputAngleRad);
        float finalInputY = Mathf.Pow(inputIntensity, 1.1f) * Mathf.Sin(finalInputAngleRad);

        /*float rad = offset * Mathf.PI / 180;

        totalAngle = Mathf.Atan(rad) + Mathf.Atan2(move.x, move.y);

        move.x = Mathf.Sin(totalAngle);
        if (move.x == 1)
        {
            move.x = 0;
        }


        move.y = Mathf.Cos(totalAngle);
        if(move.y == 1)
        {
            move.y = 0;
        }*/


        //transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(move.y / 5, transform.rotation.y, -move.x / 5, transform.rotation.w), Time.time * 0.04f);
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(finalInputX / -6, transform.rotation.y, finalInputY / -6, transform.rotation.w), Time.deltaTime * 10f);
        /*Rotate(0, -move.y);
        if (newDeltaObtained)
        {
            //transform.localRotation *= rotateBy;

            newDeltaObtained = false;
        }*/
    }

    /*public void Rotate(float rotateLeftRight, float rotateUpDown)
     {
         //Gets the world vector space for cameras up vector 
         Vector3 relativeUp = cam.transform.TransformDirection(Vector3.up);
         //Gets world vector for space cameras right vector
         Vector3 relativeRight = cam.transform.TransformDirection(Vector3.right);
 
         //Turns relativeUp vector from world to objects local space
         Vector3 objectRelativeUp = transform.InverseTransformDirection(relativeUp);
         //Turns relativeRight vector from world to object local space
         Vector3 objectRelaviveRight = transform.InverseTransformDirection(relativeRight);
         
         rotateBy = Quaternion.AngleAxis(rotateLeftRight / gameObject.transform.localScale.x, objectRelativeUp) * Quaternion.AngleAxis(-rotateUpDown / gameObject.transform.localScale.x, objectRelaviveRight);
         
         newDeltaObtained = true;
     }*/
}
