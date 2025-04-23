using UnityEngine;
using UnityEngine.InputSystem;

public class BackButtonHandler : MonoBehaviour
{
    private PlayerControls controls;
    public MenuManager menuManager;

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

        // Example: handle going back from submenus to main menu
        if (menuManager.optionsMenu.activeSelf || menuManager.logsMenu.activeSelf || menuManager.levelSelectMenu.activeSelf)
        {
            menuManager.ShowMainMenu();
        }
        else if (menuManager.mainMenu.activeSelf)
        {
            // Quit the game or do nothing
            Debug.Log("Already in Main Menu");
        }
    }
}
