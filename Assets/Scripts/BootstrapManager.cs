using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    public GameObject notificationSystemPrefab;
    public GameObject pauseMenuPrefab;

    void Awake()
    {
        // Spawn NotificationSystem if missing
        if (FindObjectOfType<NotificationManager>() == null)
        {
            GameObject notif = Instantiate(notificationSystemPrefab);
            DontDestroyOnLoad(notif);
        }

        // Spawn PauseMenuUI if missing
        if (FindObjectOfType<MenuManager>() == null)
        {
            GameObject pauseMenu = Instantiate(pauseMenuPrefab);
            DontDestroyOnLoad(pauseMenu);
        }
    }
}
