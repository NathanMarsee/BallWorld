using UnityEngine;

public enum DifficultyLevel
{
    Normal,
    Hard,
    VeryHard
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    public DifficultyLevel currentDifficulty = DifficultyLevel.Normal;

    void Awake()
    {
        if (Instance == null)
        {
            LoadDifficulty(); // 🔥 Load difficulty FIRST
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficultyFromDropdown(int index)
    {
        currentDifficulty = (DifficultyLevel)index;
        PlayerPrefs.SetInt("SavedDifficulty", index);
        PlayerPrefs.Save();
    }

    void LoadDifficulty()
    {
        int saved = PlayerPrefs.GetInt("SavedDifficulty", 0);
        currentDifficulty = (DifficultyLevel)Mathf.Clamp(saved, 0, 2);
    }

    public float GetMovementMultiplier()
    {
        return currentDifficulty switch
        {
            DifficultyLevel.Normal => 0f,
            DifficultyLevel.Hard => 1f,
            DifficultyLevel.VeryHard => 1.5f,
            _ => 0f
        };
    }

    public float GetPointMultiplier()
    {
        return currentDifficulty switch
        {
            DifficultyLevel.Normal => 1f,     // 🔥 x1
            DifficultyLevel.Hard => 2f,       // 🔥 x2
            DifficultyLevel.VeryHard => 3f,   // 🔥 x3
            _ => 1f
        };
    }

    public int GetSavedDifficultyIndex()
    {
        return (int)currentDifficulty;
    }
}
