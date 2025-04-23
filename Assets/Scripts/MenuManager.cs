using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

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

    [Header("First Selectables")]
    public GameObject mainMenuFirstButton;
    public GameObject optionsMenuFirstButton;
    public GameObject logsMenuFirstButton;
    public GameObject levelSelectMenuFirstButton;

    private Coroutine popupCoroutine;

    private void SetSelected(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        logsMenu.SetActive(false);
        levelSelectMenu.SetActive(false);

        SetSelected(mainMenuFirstButton);
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

        SetSelected(optionsMenuFirstButton);
    }

    public void ShowLogsMenu()
    {
        mainMenu.SetActive(false);
        logsMenu.SetActive(true);

        SetSelected(logsMenuFirstButton);
    }

    public void ShowLevelSelectMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);

        SetSelected(levelSelectMenuFirstButton);
    }

    public void ReturnToMainMenu()
    {
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);

        SetSelected(mainMenuFirstButton);
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
        ShowLevelSelectMenu();
    }

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
