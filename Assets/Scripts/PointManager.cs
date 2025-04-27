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

        // 🔥 No more double multiplier here
        playerPoints += amount;
        SavePoints();
        UpdatePointsUI();

        SoundManager.Instance?.PlayPointSound();
        NotificationManager.Instance?.ShowNotification($"+{amount} Point{(amount == 1 ? "" : "s")}!");

        CheckForUnlocks();
    }

    public void AddPoint()
    {
        AddPoints(1);
    }

    public void ResetPoints()
    {
        // 🔥 Full reset
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        playerPoints = 0;
        PlayerPrefs.SetInt("PlayerPoints", playerPoints);
        PlayerPrefs.SetString("UnlockedBalls", "0");
        PlayerPrefs.SetInt("SelectedBall", 0);
        PlayerPrefs.SetInt("SelectedBallIndex", 0);
        PlayerPrefs.SetInt("InfiniteRunnerHighScore", 0);
        PlayerPrefs.Save();

        UpdatePointsUI();

        NotificationManager.Instance?.ShowNotification("Game data has been fully reset!");
        SoundManager.Instance?.PlayPointResetSound();

        FindObjectOfType<LogMenuManager>()?.Refresh();
    }

    void UpdatePointsUI()
    {
        if (pointsText != null)
        {
            pointsText.text = $"Points: {playerPoints}";
        }
        else
        {
            Debug.LogWarning("PointManager: pointsText not assigned. Skipping UI update.");
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

    public bool SpendPoints(int amount)
    {
        if (amount <= 0) return false;

        if (playerPoints >= amount)
        {
            playerPoints -= amount;
            SavePoints();
            UpdatePointsUI();

            SoundManager.Instance?.PlayPointSound();
            NotificationManager.Instance?.ShowNotification($"You spent {amount} point{(amount == 1 ? "" : "s")}!");

            return true;
        }
        else
        {
            NotificationManager.Instance?.ShowNotification("Not enough points!");
            SoundManager.Instance?.PlayErrorSound();
            return false;
        }
    }
}
