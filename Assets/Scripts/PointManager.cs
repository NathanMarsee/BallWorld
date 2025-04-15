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
        // Singleton logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
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
        NotifyLogMenu();  // 🔄 Refresh log button state
    }

    public void ResetPoints()
    {
        playerPoints = 0;
        SavePoints();
        UpdatePointsUI();
        NotifyLogMenu();  // 🔄 Refresh log button state
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
            logMenu.RefreshLogList(); // Re-check point requirements and update buttons
        }
    }
}
