using UnityEngine;
using UnityEngine.SceneManagement;

public class HoopScoreTrigger : MonoBehaviour
{
    [Header("Scoring")]
    public int pointsToAward = 10;
    public float delayBeforeRestart = 3f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Add points through PointManager
            for (int i = 0; i < pointsToAward; i++)
            {
                PointManager.Instance.AddPoint();
            }

            // Show score popup using MenuManager
            MenuManager menu = FindObjectOfType<MenuManager>();
            if (menu != null)
            {
                menu.ShowScorePopup(pointsToAward);
            }

            // Restart scene after delay
            Invoke(nameof(RestartLevel), delayBeforeRestart);
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
