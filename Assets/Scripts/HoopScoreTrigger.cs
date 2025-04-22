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

    private void Start()
    {
        if (pointsLabel != null)
        {
            pointsLabel.text = $"{pointsToAward}";
        }
        else
        {
            Debug.LogWarning("Points label not assigned in inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

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

        // Show popup if available
        MenuManager menu = FindObjectOfType<MenuManager>();
        if (menu != null)
        {
            menu.ShowScorePopup(pointsToAward);
        }

        // Restart level after short delay
        Invoke(nameof(RestartLevel), delayBeforeRestart);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
