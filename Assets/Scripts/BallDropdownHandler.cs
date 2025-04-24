using UnityEngine;
using TMPro; // ✅ Required for TMP_Dropdown
using System.Collections.Generic;

public class BallDropdownHandler : MonoBehaviour
{
    [Header("References")]
    public TMP_Dropdown ballDropdown;
    public BallDatabase ballDatabase;

    void Start()
    {
        if (ballDropdown == null)
        {
            Debug.LogError("BallDropdownHandler: TMP_Dropdown not assigned.");
            return;
        }

        if (ballDatabase == null)
        {
            Debug.LogError("BallDropdownHandler: BallDatabase not assigned.");
            return;
        }

        PopulateDropdown();

        // Load previously selected index from PlayerPrefs
        if (PlayerPrefs.HasKey("SelectedBall"))
        {
            int savedIndex = PlayerPrefs.GetInt("SelectedBall");
            ballDropdown.value = savedIndex;
            BallSelector.Instance?.SelectBall(savedIndex);
        }

        ballDropdown.onValueChanged.AddListener(OnBallSelected);
    }

    void PopulateDropdown()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < ballDatabase.ballPrefabs.Length; i++)
        {
            var prefab = ballDatabase.ballPrefabs[i];
            string label = prefab != null ? prefab.name : $"Ball {i}";
            options.Add(new TMP_Dropdown.OptionData(label));
        }

        ballDropdown.ClearOptions();
        ballDropdown.AddOptions(options);
    }

    void OnBallSelected(int index)
    {
        BallSelector.Instance?.SelectBall(index);
        PlayerPrefs.SetInt("SelectedBall", index);
        PlayerPrefs.Save();

        Debug.Log($"BallDropdownHandler: Ball {index} selected.");
    }
}
