using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HoopScoreTrigger : MonoBehaviour
{
    [Header("Scoring")]
    public int pointsToAward = 10;
    public float delayBeforeRestart = 0.5f;

    [Header("Visual Label")]
    public TextMeshPro pointsLabel;

    private bool triggered = false;
    private float lastMultiplier = -1f; // 🔥 Track the last multiplier separately

    private void Start()
    {
        UpdatePointsLabel(); // Always call this at start
    }

    private void Update()
    {
        // 🔥 Constantly check if difficulty multiplier changed
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
        float points = pointsToAward;

        float actualPoints = points * multiplier;
        pointsLabel.text = $"{actualPoints:0}"; // show no decimal places, but don't RoundToInt
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

        float multiplier = DifficultyManager.Instance?.GetPointMultiplier() ?? 1f;
        int actualPoints = Mathf.RoundToInt(pointsToAward * multiplier);

        (PointManager.Instance ?? FindObjectOfType<PointManager>())?.AddPoints(actualPoints);

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
            menu.ShowScorePopup(actualPoints);
        }

        Invoke(nameof(RestartLevel), delayBeforeRestart);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
