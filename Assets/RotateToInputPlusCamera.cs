using UnityEngine;
using UnityEngine.InputSystem;

public class RotateToInputPlusCamera : MonoBehaviour
{
    public GameObject cam;
    public float offset;
    public Vector2 move;
    public float inputAngle;
    public float finalInputAngle;
    public float inputIntensity;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls(); // create input instance
    }

    void OnEnable()
    {
        controls.Enable(); // enable the bindings
    }

    void OnDisable()
    {
        controls.Disable(); // disable on destroy/disable
    }

    void FixedUpdate()
    {
        Quaternion currentRotation = transform.rotation;

        move = controls.Gameplay.Move.ReadValue<Vector2>();
        Debug.Log($"🎮 Stick Input: {move}");

        if (move.sqrMagnitude < 0.01f)
        {
            Debug.Log("🧊 Input too small, skipping rotation.");
            return;
        }

        inputIntensity = move.sqrMagnitude;
        Debug.Log($"📈 Input Intensity: {inputIntensity}");

        offset = 90f - cam.transform.rotation.eulerAngles.y;
        Debug.Log($"🎥 Camera Y Rotation: {cam.transform.rotation.eulerAngles.y}, Offset: {offset}");

        inputAngle = Mathf.Atan2(-move.y, -move.x) * Mathf.Rad2Deg;
        if (inputAngle < 0)
            inputAngle += 360f;

        Debug.Log($"🧭 Input Angle: {inputAngle}");

        finalInputAngle = (inputAngle + offset) % 360f;
        Debug.Log($"📐 Final Input Angle: {finalInputAngle}");

        float finalInputAngleRad = finalInputAngle * Mathf.Deg2Rad;

        float finalInputX = Mathf.Pow(inputIntensity, 1.1f) * Mathf.Cos(finalInputAngleRad);
        float finalInputY = Mathf.Pow(inputIntensity, 1.1f) * Mathf.Sin(finalInputAngleRad);
        Debug.Log($"🎯 Calculated Input: X={finalInputX}, Y={finalInputY}");

        Quaternion targetRotation = new Quaternion(
            finalInputX / -6f,
            currentRotation.y,
            finalInputY / -6f,
            currentRotation.w
        );

        Debug.Log($"🔄 Applying rotation: {targetRotation.eulerAngles}");
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * 10f);
    }
}
