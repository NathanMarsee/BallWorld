using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject logsMenu;
    public GameObject levelSelectMenu;

    [Header("Popup")]
    public GameObject scorePopup;
    public TMP_Text popupText;
    public float popupDuration = 2f;

    private Coroutine popupCoroutine;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        logsMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ShowLogsMenu()
    {
        mainMenu.SetActive(false);
        logsMenu.SetActive(true);
    }

    public void ShowLevelSelectMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadInfiniteRunnerScene()
    {
        SceneManager.LoadScene("InfiniteRunner");
    }

    public void LoadBasketballScene()
    {
        SceneManager.LoadScene("Basketball");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void PlayGame()
    {
        ShowLevelSelectMenu(); // Now opens the level select screen
    }

    // ✅ New method to show the popup
    public void ShowScorePopup(int points)
    {
        if (scorePopup != null && popupText != null)
        {
            popupText.text = $"+{points} Points!";
            scorePopup.SetActive(true);

            if (popupCoroutine != null)
                StopCoroutine(popupCoroutine);

            popupCoroutine = StartCoroutine(HidePopupAfterDelay(popupDuration));
        }
    }

    private System.Collections.IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        scorePopup.SetActive(false);
    }
}
