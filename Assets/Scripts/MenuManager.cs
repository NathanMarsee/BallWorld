using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject logsMenu;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        logsMenu.SetActive(false);
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

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void PlayGame()
    {
        // Placeholder - later you can load a level
        Debug.Log("Play Game Clicked");
    }
}
