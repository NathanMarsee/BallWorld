using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    private int currentPoints = 0;
    private const string PlayerPrefsKey = "PlayerPoints";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // ensure this stays across scenes
        currentPoints = PlayerPrefs.GetInt(PlayerPrefsKey, 0);
        UpdateDisplay();
    }

    public void AddPoints(int amount)
    {
        currentPoints += amount;
        PlayerPrefs.SetInt(PlayerPrefsKey, currentPoints);
        PlayerPrefs.Save();
        UpdateDisplay();
    }

    public void ResetPoints()
    {
        currentPoints = 0;
        PlayerPrefs.DeleteKey(PlayerPrefsKey);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (pointsText != null)
            pointsText.text = $"Points: {currentPoints}";
    }

    public int GetPoints()
    {
        return currentPoints;
    }
}
