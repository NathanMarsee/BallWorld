using UnityEngine;

public class TutorialPopupController : MonoBehaviour
{
    public GameObject popupPanel; 

    private const string TutorialShownKey = "LevelTutorialShown";

    void Start()
    {
        if (PointManager.Instance != null &&
            PointManager.Instance.CurrentPoints == 0 &&
            !PlayerPrefs.HasKey(TutorialShownKey))
        {
            popupPanel.SetActive(true);
            Time.timeScale = 0f; 

            PlayerPrefs.SetInt(TutorialShownKey, 1); 
            PlayerPrefs.Save();
        }
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
