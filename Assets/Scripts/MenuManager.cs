using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections; // <-- Needed for Coroutine

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
    public GameObject logEntryFirstButton;
    public GameObject logCloseButton; // 🔥 Close button field

    [Header("Scene-Specific UI")]
    public GameObject basketballCanvas;

    private Coroutine popupCoroutine;

    void OnEnable()
    {
        UpdateBasketballCanvas();
    }

    void UpdateBasketballCanvas()
    {
        bool isBasketballScene = SceneManager.GetActiveScene().name == "Basketball";
        bool otherMenusOpen = optionsMenu.activeSelf || logsMenu.activeSelf || levelSelectMenu.activeSelf;

        if (basketballCanvas != null)
            basketballCanvas.SetActive(isBasketballScene && !otherMenusOpen);
    }

    private void SetSelected(GameObject button)
    {
        if (button == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        logsMenu.SetActive(false);
        levelSelectMenu.SetActive(false);

        UpdateBasketballCanvas();
        SetSelected(mainMenuFirstButton);
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

        UpdateBasketballCanvas();
        SetSelected(optionsMenuFirstButton);
    }

    public void ShowLogsMenu()
    {
        mainMenu.SetActive(false);
        logsMenu.SetActive(true);

        UpdateBasketballCanvas();
        SetSelected(logsMenuFirstButton);
    }

    public void ShowLevelSelectMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);

        UpdateBasketballCanvas();
        SetSelected(levelSelectMenuFirstButton);
    }

    public void ReturnToMainMenu()
    {
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);

        UpdateBasketballCanvas();
        SetSelected(mainMenuFirstButton);
    }

    public void LoadTutorial() => SceneManager.LoadScene("Tutorial");
    public void LoadInfiniteRunnerScene() => SceneManager.LoadScene("InfiniteRunner");
    public void LoadBasketballScene() => SceneManager.LoadScene("Basketball");
    public void LoadExtractionScene() => SceneManager.LoadScene("Extraction");

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

    public void SetFirstLogEntryButton()
    {
        if (logEntryFirstButton != null)
            StartCoroutine(DelaySelect(logEntryFirstButton)); // 🔥 FIX: delay
    }

    public void SetLogCloseButton()
    {
        if (logCloseButton != null)
            StartCoroutine(DelaySelect(logCloseButton)); // 🔥 FIX: delay
    }

    private IEnumerator DelaySelect(GameObject button)
    {
        yield return null; // wait 1 frame
        SetSelected(button);
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        scorePopup.SetActive(false);
    }

    public void ReturnToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
