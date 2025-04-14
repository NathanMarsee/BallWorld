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

    [Header("Managers")]
    public MenuManager menuManager;
    public OptionsManager optionsManager;
    public LogMenuManager logManager;

    private bool isPaused = false;
    private bool isMainMenu = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Initial scene setup
        HandleScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (isMainMenu) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current?.startButton.wasPressedThisFrame == true)
        {
            if (isPaused)
                ResumeGame();
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
            Time.timeScale = 1f;
        }
        else
        {
            DisablePauseMenu();
            CloseAllPanels(); // Clean slate
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void PauseGame()
    {
        EnablePauseMenu();
        menuManager?.ShowMainMenu();
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        DisablePauseMenu();
        isPaused = false;
        Time.timeScale = 1f;
    }

    private void EnablePauseMenu()
    {
        if (canvas != null) canvas.SetActive(true);
    }

    private void DisablePauseMenu()
    {
        if (canvas != null) canvas.SetActive(false);
    }

    private void CloseAllPanels()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (logsPanel != null) logsPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
    }
}
