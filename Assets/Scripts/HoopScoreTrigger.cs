using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HoopScoreTrigger : MonoBehaviour
{
    [Header("Scoring")]
    public int basePoints = 10;                 // Base unmodified points
    public float delayBeforeRestart = 0.5f;      // Delay before reload after scoring

    [Header("Visual Label")]
    public TextMeshPro pointsLabel;

    private bool triggered = false;
    private float lastMultiplier = -1f;
    private int pointsToAward = 0;               // ✅ Pre-calculated, correct points to award

    private void Start()
    {
        UpdatePointsLabel();
    }

    private void Update()
    {
        // Constantly check if difficulty changed
        float currentMultiplier = DifficultyManager.Instance?.GetPointMultiplier() ?? 1f;
        if (!Mathf.Approximately(currentMultiplier, lastMultiplier))
        {
            UpdatePointsLabel();
        }
    }

    private void UpdatePointsLabel()
    {
        if (pointsLabel != null)
        {
            float multiplier = DifficultyManager.Instance?.GetPointMultiplier() ?? 1f;
            pointsToAward = Mathf.RoundToInt(basePoints * multiplier); // 🔥 Store correct award points
            pointsLabel.text = $"{pointsToAward}";
            lastMultiplier = multiplier;
        }
        else
        {
            Debug.LogWarning("HoopScoreTrigger: Points label not assigned in inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // ✅ Just use pre-calculated pointsToAward without re-multiplying
        (PointManager.Instance ?? FindObjectOfType<PointManager>())?.AddPoints(pointsToAward);

        GameObject rotationGuide = GameObject.Find("OrbRotationGuide");
        if (rotationGuide != null)
        {
            var follow = rotationGuide.GetComponent<FollowObject>();
            var rotate = rotationGuide.GetComponent<RotateAfterVelocity>();

            if (follow != null) follow.enabled = false;
            if (rotate != null) rotate.enabled = false;
        }
        else
        {
            Debug.LogWarning("OrbRotationGuide not found in scene.");
        }

        MenuManager menu = FindObjectOfType<MenuManager>();
        if (menu != null)
        {
            menu.ShowScorePopup(pointsToAward);
        }

        Invoke(nameof(RestartLevel), delayBeforeRestart);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
