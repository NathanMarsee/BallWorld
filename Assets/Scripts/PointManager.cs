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
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0) return;

        playerPoints += amount;
        SavePoints();
        UpdatePointsUI();
        NotifyLogMenu();

        SoundManager.Instance?.PlayPointSound();
        NotificationManager.Instance?.ShowNotification($"You gained {amount} points!");
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

    void NotifyLogMenu()
    {
        var logMenu = FindObjectOfType<LogMenuManager>();
        if (logMenu != null && logMenu.isActiveAndEnabled)
        {
            logMenu.RefreshLogList();
        }
    }
}
