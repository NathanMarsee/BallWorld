using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    // This function can be linked directly in the Button's OnClick() event.
    public void LoadScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
