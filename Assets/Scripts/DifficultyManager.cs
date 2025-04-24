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
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDifficulty();
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
            DifficultyLevel.Normal => 1f,
            DifficultyLevel.Hard => 1.5f,
            DifficultyLevel.VeryHard => 2f,
            _ => 1f
        };
    }

    public int GetSavedDifficultyIndex() => (int)currentDifficulty;
}
