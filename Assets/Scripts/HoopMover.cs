using UnityEngine;

public class HoopMover : MonoBehaviour
{
    public enum MoveDirection { Vertical, Horizontal }

    [Header("Movement Settings")]
    public MoveDirection moveDirection = MoveDirection.Vertical;
    public float moveDistance = 2f;
    public float moveSpeed = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float multiplier = DifficultyManager.Instance?.GetMovementMultiplier() ?? 0f;

        if (multiplier <= 0f)
            return;

        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance * multiplier;

        if (moveDirection == MoveDirection.Vertical)
            transform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
        else
            transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
