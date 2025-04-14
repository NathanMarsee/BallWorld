using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateToInputPlusCamera : MonoBehaviour
{
    public GameObject cam;
    private PlayerControls controls;

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
    controls = new PlayerControls();
    controls.Enable();

    if (cam == null)
    {
        cam = Camera.main?.gameObject;
        if (cam == null)
            Debug.LogWarning("RotateToInputPlusCamera: Camera not assigned and could not find MainCamera.");
    }
}

    void FixedUpdate()
    {
        thisAngle = transform.rotation;
        var gamepad = controls.Gameplay.Move.ReadValue<Vector2>();
        if (gamepad == null)
            return;

        move = controls.Gameplay.Move.ReadValue<Vector2>();
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

        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(finalInputX / -6, transform.rotation.y, finalInputY / -6, transform.rotation.w), Time.deltaTime * 10f);
    }
    void OnDestroy()
{
    controls.Disable();
}


}
