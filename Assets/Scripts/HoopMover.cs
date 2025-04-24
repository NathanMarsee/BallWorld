using UnityEngine;

public class HoopMover : MonoBehaviour
{
    public enum MoveDirection { None, Vertical, Horizontal }

    [System.Serializable]
    public class DifficultyMovementSettings
    {
        public float moveDistance = 2f;
        public float moveSpeed = 1f;
    }

    [Header("Settings Per Difficulty")]
    public DifficultyMovementSettings normalSettings = new DifficultyMovementSettings();
    public DifficultyMovementSettings hardSettings = new DifficultyMovementSettings();
    public DifficultyMovementSettings veryHardSettings = new DifficultyMovementSettings();

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        var difficulty = DifficultyManager.Instance?.currentDifficulty ?? DifficultyLevel.Normal;

        // Choose settings and direction based on difficulty
        DifficultyMovementSettings settings;
        MoveDirection direction;

        switch (difficulty)
        {
            case DifficultyLevel.Hard:
                settings = hardSettings;
                direction = MoveDirection.Vertical;
                break;
            case DifficultyLevel.VeryHard:
                settings = veryHardSettings;
                direction = MoveDirection.Horizontal;
                break;
            default:
                settings = normalSettings;
                direction = MoveDirection.None;
                break;
        }

        // Exit if no movement should occur
        if (direction == MoveDirection.None || settings.moveDistance == 0f || settings.moveSpeed == 0f)
            return;

        // Compute movement offset
        float offset = Mathf.Sin(Time.time * settings.moveSpeed) * settings.moveDistance;

        if (direction == MoveDirection.Vertical)
            transform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
        else
            transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
