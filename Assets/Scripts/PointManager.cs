using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }

    public TMP_Text pointsText;
    private int playerPoints = 0;

    public int CurrentPoints => playerPoints;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadPoints();
        UpdatePointsUI();
    }

    public void AddPoint()
    {
        playerPoints++;
        SavePoints();
        UpdatePointsUI();
        NotifyLogMenu();

        SoundManager.Instance?.PlayPointSound();
        NotificationManager.Instance?.ShowNotification("You gained 1 point!");

        CheckForUnlocks(); 
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0) return;

        playerPoints += amount;
        SavePoints();
        UpdatePointsUI();
        NotifyLogMenu();

        SoundManager.Instance?.PlayPointSound();
        NotificationManager.Instance?.ShowNotification($"You gained {amount} point{(amount == 1 ? "" : "s")}!");

        CheckForUnlocks(); 
    }

    public void ResetPoints()
    {
        playerPoints = 0;
        SavePoints();
        UpdatePointsUI();
        NotifyLogMenu();

        SoundManager.Instance?.PlayPointResetSound();
        NotificationManager.Instance?.ShowNotification("Points have been reset.");
    }

    void UpdatePointsUI()
    {
        if (pointsText != null)
            pointsText.text = $"Points: {playerPoints}";
    }

    void SavePoints()
    {
        PlayerPrefs.SetInt("PlayerPoints", playerPoints);
        PlayerPrefs.Save();
    }

    void LoadPoints()
    {
        playerPoints = PlayerPrefs.GetInt("PlayerPoints", 0);
    }
    public void ResetAllData()
{
    PlayerPrefs.DeleteAll();       
    playerPoints = 0;
    UpdatePointsUI();              

    NotificationManager.Instance?.ShowNotification("Game data has been reset.");
    SoundManager.Instance?.PlayPointResetSound();

    FindObjectOfType<LogMenuManager>()?.Refresh();
}


    void NotifyLogMenu()
{
    var logMenu = FindObjectOfType<LogMenuManager>();
    if (logMenu != null)
    {
        bool changesMade = false;

        foreach (var log in logMenu.logEntries)
        {
            if (!log.unlocked && playerPoints >= log.pointsRequired)
            {
                log.unlocked = true;

                
                if (log.pointsRequired > 0)
                {
                    NotificationManager.Instance?.ShowNotification($"New log unlocked: \"{log.title}\"");
                    SoundManager.Instance?.PlayUnlockSound();
                }

                changesMade = true;
            }
        }

        if (changesMade)
        {
            logMenu.RefreshLogList(); 
        }
    }
}


    private void CheckForUnlocks()
{
    var logMenu = FindObjectOfType<LogMenuManager>();
    if (logMenu == null) return;

    bool unlockedAny = false;

    foreach (var log in logMenu.logEntries)
    {
        if (!log.unlocked && playerPoints >= log.pointsRequired)
        {
            log.unlocked = true;

            logMenu.SaveUnlockedLog(log); 

            if (log.pointsRequired > 0)
            {
                NotificationManager.Instance?.ShowNotification($"New log unlocked: \"{log.title}\"");
                SoundManager.Instance?.PlayUnlockSound();
            }

            unlockedAny = true;
        }
    }

    if (unlockedAny && logMenu.isActiveAndEnabled)
    {
        logMenu.RefreshLogList();
    }
}

}
