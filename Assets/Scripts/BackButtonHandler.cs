using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BackButtonHandler : MonoBehaviour
{
    private PlayerControls controls;
    public MenuManager menuManager;
    public PauseManager pauseManager;

    [Header("Optional Canvas References")]
    public GameObject basketballCanvas; // 🔁 Assign in Inspector

    void Awake()
    {
        controls = new PlayerControls();
        controls.UI.Cancel.performed += ctx => HandleBack();
    }

    void OnEnable()
    {
        controls.UI.Enable();
    }

    void OnDisable()
    {
        controls.UI.Disable();
    }

    void HandleBack()
    {
        if (menuManager == null || pauseManager == null) return;

        if (Time.timeScale == 0f) // 🔥 If paused
        {
            pauseManager.ResumeGameAndCloseMenus(); // ✅ CORRECT METHOD
            return;
        }

        // Not paused, handle basketballCanvas
        if (basketballCanvas != null && basketballCanvas.activeSelf)
        {
            basketballCanvas.SetActive(false);
            return;
        }

        // Handle submenus normally if needed
        if (menuManager.optionsMenu.activeSelf ||
            menuManager.logsMenu.activeSelf ||
            menuManager.levelSelectMenu.activeSelf)
        {
            menuManager.ShowMainMenu();
        }
        else if (menuManager.mainMenu.activeSelf && SceneManager.GetActiveScene().name != "MainMenu")
        {
            pauseManager.ResumeGameAndCloseMenus();
        }

    }
}
