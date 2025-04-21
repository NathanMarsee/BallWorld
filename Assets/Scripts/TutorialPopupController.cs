using UnityEngine;

public class TutorialPopupController : MonoBehaviour
{
    public GameObject popupPanel; // Assign your tutorial popup UI here

    private const string TutorialShownKey = "LevelTutorialShown";

    void Start()
    {
        if (PointManager.Instance != null &&
            PointManager.Instance.CurrentPoints == 0
            /* && !PlayerPrefs.HasKey(TutorialShownKey) */) // ← commented out for dev
        {
            popupPanel.SetActive(true);
            Time.timeScale = 0f; // ⛔ Pause gameplay

            // PlayerPrefs.SetInt(TutorialShownKey, 1); // ← disable saving until release
            // PlayerPrefs.Save();
        }
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f; // ▶️ Resume gameplay
    }
}
