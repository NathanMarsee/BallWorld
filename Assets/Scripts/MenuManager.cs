using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject logsMenu;
    public GameObject levelSelectMenu;

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
    public void LoadExtractionScene()
    {
        SceneManager.LoadScene("Extraction");
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
}
