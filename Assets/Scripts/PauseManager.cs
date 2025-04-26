using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject canvas;
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject logsPanel;
    public GameObject levelSelectPanel;
    public GameObject basketballDifficultyMenu;

    [Header("Managers")]
    public MenuManager menuManager;
    public OptionsManager optionsManager;
    public LogMenuManager logManager;

    private bool isPaused = false;
    private bool isMainMenu = false;
    private bool suppressNextPauseInput = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        int index = PlayerPrefs.GetInt("ResolutionIndex", -1);
        if (index >= 0)
        {
            Resolution[] resolutions = Screen.resolutions;
            if (index < resolutions.Length)
            {
                var res = resolutions[index];
                Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        HandleScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (isMainMenu) return;

        if (suppressNextPauseInput)
        {
            suppressNextPauseInput = false;
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current?.startButton.wasPressedThisFrame == true)
        {
            if (isPaused)
                ResumeGameAndCloseMenus();
            else
                PauseGame();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleScene(scene.name);
    }

    void HandleScene(string sceneName)
    {
        isMainMenu = sceneName == "MainMenu";

        if (isMainMenu)
        {
            EnablePauseMenu();
            menuManager?.ShowMainMenu();
            isPaused = true;
            Time.timeScale = 1.2f;
        }
        else
        {
            DisablePauseMenu();
            CloseAllPanels();
            isPaused = false;
            Time.timeScale = 1.2f;
        }
    }

    public void PauseGame()
    {
        if (isMainMenu) return;

        EnablePauseMenu();
        menuManager?.ShowMainMenu();
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (isMainMenu) return;

        DisablePauseMenu();
        isPaused = false;
        Time.timeScale = 1.2f;
    }

    public void ResumeGameAndCloseMenus()
    {
        if (isMainMenu) return;

        CloseAllPanels();
        DisablePauseMenu();
        isPaused = false;
        Time.timeScale = 1.2f;
        suppressNextPauseInput = true;
    }

    private void EnablePauseMenu()
    {
        if (canvas != null)
            canvas.SetActive(true);
    }

    private void DisablePauseMenu()
    {
        if (canvas != null)
            canvas.SetActive(false);
    }

    private void CloseAllPanels()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (logsPanel != null) logsPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (basketballDifficultyMenu != null) basketballDifficultyMenu.SetActive(false);
    }
}
