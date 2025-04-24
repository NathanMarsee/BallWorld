using UnityEngine;
using UnityEngine.InputSystem;

public class BackButtonHandler : MonoBehaviour
{
    private PlayerControls controls;
    public MenuManager menuManager;

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
        if (menuManager == null) return;

        // Close basketballCanvas if it's open
        if (basketballCanvas != null && basketballCanvas.activeSelf)
        {
            basketballCanvas.SetActive(false);
            return;
        }

        // Handle submenus
        if (menuManager.optionsMenu.activeSelf || 
            menuManager.logsMenu.activeSelf || 
            menuManager.levelSelectMenu.activeSelf)
        {
            menuManager.ShowMainMenu();
        }
        else if (menuManager.mainMenu.activeSelf)
        {
            Debug.Log("Already in Main Menu");
        }
    }
}
