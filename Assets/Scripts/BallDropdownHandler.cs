using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class BallDropdownHandler : MonoBehaviour
{
    [Header("References")]
    public TMP_Dropdown ballDropdown;
    public BallDatabase ballDatabase;
    public Button buyEquipButton;
    public TMP_Text buyEquipButtonText;

    private HashSet<int> unlockedBalls = new HashSet<int>();
    private const string UnlockedBallsKey = "UnlockedBalls";
    private const string SelectedBallKey = "SelectedBall";

    private int currentBallIndex = 0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Persist across menus
    }

    void Start() 
    {
        TryReconnectReferences();
    }

    void TryReconnectReferences()
    {
        if (ballDropdown == null)
            ballDropdown = FindObjectOfType<TMP_Dropdown>();

        if (buyEquipButton == null)
            buyEquipButton = FindObjectOfType<Button>();

        if (buyEquipButtonText == null && buyEquipButton != null)
            buyEquipButtonText = buyEquipButton.GetComponentInChildren<TMP_Text>();

        if (ballDatabase == null)
            ballDatabase = FindObjectOfType<BallDatabase>();

        if (ballDropdown != null)
        {
            ballDropdown.onValueChanged.RemoveAllListeners();
            ballDropdown.onValueChanged.AddListener(OnBallSelected);

            PopulateDropdown();
            LoadUnlockedBalls();

            currentBallIndex = PlayerPrefs.GetInt(SelectedBallKey, 0);
            ballDropdown.value = currentBallIndex;
            UpdateBuyEquipButton(currentBallIndex);
        }

        if (buyEquipButton != null)
        {
            buyEquipButton.onClick.RemoveAllListeners();
            buyEquipButton.onClick.AddListener(BuyOrEquipSelectedBall);
        }
    }

    void PopulateDropdown()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (var ball in ballDatabase.balls)
        {
            string label = ball.prefab != null ? ball.prefab.name : "Unnamed Ball";
            options.Add(new TMP_Dropdown.OptionData(label));
        }

        ballDropdown.ClearOptions();
        ballDropdown.AddOptions(options);
    }

    void OnBallSelected(int index)
    {
        UpdateBuyEquipButton(index);
        currentBallIndex = index;

        PlayerPrefs.SetInt(SelectedBallKey, index);
        PlayerPrefs.Save();

        BallPreviewManager.RefreshPreview(); // 🔥 Refresh the already-existing preview
    }

    void UpdateBuyEquipButton(int index)
    {
        if (IsUnlocked(index))
            buyEquipButtonText.text = "Equip";
        else
            buyEquipButtonText.text = $"Buy ({ballDatabase.balls[index].cost} pts)";
    }

    void BuyOrEquipSelectedBall()
    {
        int index = ballDropdown.value;

        if (BallSelector.Instance == null)
        {
            Debug.LogError("BallSelector.Instance is null! Cannot equip or buy ball.");
            NotificationManager.Instance?.ShowNotification("Error: Ball system not ready.");
            SoundManager.Instance?.PlayErrorSound();
            return;
        }

        if (IsUnlocked(index))
        {
            BallSelector.Instance.SelectBall(index);
            PlayerPrefs.SetInt(SelectedBallKey, index);
            PlayerPrefs.Save();
            Debug.Log($"Equipped ball: {ballDatabase.balls[index].prefab.name}");
        }
        else
        {
            int cost = ballDatabase.balls[index].cost;
            if (PointManager.Instance.CurrentPoints >= cost)
            {
                PointManager.Instance.SpendPoints(cost);
                UnlockBall(index);
                BallSelector.Instance.SelectBall(index);
                PlayerPrefs.SetInt(SelectedBallKey, index);
                PlayerPrefs.Save();
                Debug.Log($"Bought and equipped ball: {ballDatabase.balls[index].prefab.name}");
            }
            else
            {
                NotificationManager.Instance?.ShowNotification("Not enough points!");
                SoundManager.Instance?.PlayErrorSound();
            }
        }

        UpdateBuyEquipButton(index);
    }

    private void UnlockBall(int index)
    {
        unlockedBalls.Add(index);
        SaveUnlockedBalls();
    }

    private bool IsUnlocked(int index)
    {
        return unlockedBalls.Contains(index);
    }

    private void SaveUnlockedBalls()
    {
        string saveString = string.Join("|", unlockedBalls);
        PlayerPrefs.SetString(UnlockedBallsKey, saveString);
        PlayerPrefs.Save();
    }

    private void LoadUnlockedBalls()
    {
        if (PlayerPrefs.HasKey(UnlockedBallsKey))
        {
            string saved = PlayerPrefs.GetString(UnlockedBallsKey);
            string[] split = saved.Split('|');
            foreach (var s in split)
            {
                if (int.TryParse(s, out int index))
                    unlockedBalls.Add(index);
            }
        }
        else
        {
            unlockedBalls.Add(0); // Unlock first ball by default
            SaveUnlockedBalls();
        }
    }
}
