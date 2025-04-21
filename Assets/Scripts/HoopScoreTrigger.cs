using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HoopScoreTrigger : MonoBehaviour
{
    [Header("Scoring")]
    public int pointsToAward = 10;
    public float delayBeforeRestart = 0.1f;

    [Header("Visual Label")]
    public TextMeshPro pointsLabel;

    private bool triggered = false;

    private void Start()
    {
        // If the label was assigned, update the text
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

        if (other.CompareTag("Player"))
        {
            triggered = true;

            for (int i = 0; i < pointsToAward; i++)
            {
                PointManager.Instance.AddPoint();
            }

            MenuManager menu = FindObjectOfType<MenuManager>();
            if (menu != null)
            {
                menu.ShowScorePopup(pointsToAward);
            }

            Invoke(nameof(RestartLevel), delayBeforeRestart);
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
