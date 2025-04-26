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

    public void AddPoints(int amount)
    {
        if (amount <= 0) return;

        float multiplier = DifficultyManager.Instance?.GetPointMultiplier() ?? 1f;
        int adjusted = Mathf.RoundToInt(amount * multiplier);

        playerPoints += adjusted;
        SavePoints();
        UpdatePointsUI();

        SoundManager.Instance?.PlayPointSound();
        NotificationManager.Instance?.ShowNotification($"You gained {adjusted} point{(adjusted == 1 ? "" : "s")}!");

        CheckForUnlocks();
    }

    public void AddPoint()
    {
        AddPoints(1); // Delegate to the real method
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
        {
            pointsText.text = $"Points: {playerPoints}";
        }
        else
        {
            Debug.Log("PointManager: pointsText is not assigned in this scene. Skipping UI update.");
        }
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

    // 🔥 NEW: Spend Points Safely
    public bool SpendPoints(int amount)
    {
        if (amount <= 0) return false;

        if (playerPoints >= amount)
        {
            playerPoints -= amount;
            SavePoints();
            UpdatePointsUI();

            SoundManager.Instance?.PlayPointSound(); // Optional: reuse point sound
            NotificationManager.Instance?.ShowNotification($"You spent {amount} point{(amount == 1 ? "" : "s")}!");

            return true;
        }
        else
        {
            NotificationManager.Instance?.ShowNotification("Not enough points!");
            SoundManager.Instance?.PlayErrorSound(); // Optional if you have an error sound
            return false;
        }
    }
}
