using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class LogMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject logButtonPrefab;
    public Transform logListParent;
    public GameObject logDisplayPanel;
    public TMP_Text logDisplayText;
    public TMP_Text pointsText;
    public GameObject closeButton;
    public GameObject logTabBar; // ✅ Added reference for tab bar

    [Header("Background Elements to Disable")]
    public GameObject logScrollView;
    public GameObject backButton;

    [Header("Tab Buttons")]
    public Button tutorialTabButton;
    public Button johnTabButton;
    public Button cynthiaTabButton;
    public Button stevenTabButton;

    [Header("Log Data")]
    public List<LogEntry> logEntries;

    private PointManager pointManager;
    private LogCategory currentCategory = LogCategory.Tutorial;

    void OnEnable()
    {
        pointManager = PointManager.Instance ?? FindObjectOfType<PointManager>();

        if (pointManager == null)
        {
            Debug.LogError("LogMenuManager: No PointManager found in scene.");
            return;
        }

        LoadUnlockedLogs(); 

        SetupTabButtons();
        ResetLogView();
        RefreshLogList();
        UpdatePointsUI();
    }

    void SetupTabButtons()
    {
        tutorialTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Tutorial));
        johnTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Character_John));
        cynthiaTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Character_Cynthia));
        stevenTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Character_Steven));
    }

    void ChangeCategory(LogCategory category)
    {
        currentCategory = category;
        RefreshLogList();
    }

    void UpdatePointsUI()
    {
        if (pointManager != null)
            pointsText.text = $"Points: {pointManager.CurrentPoints}";
    }

    public void RefreshLogList()
    {
        foreach (Transform child in logListParent)
        {
            Destroy(child.gameObject);
        }

        var filteredLogs = logEntries
            .Where(log => log.category == currentCategory && log.unlocked);

        foreach (var log in filteredLogs)
        {
            GameObject newButton = Instantiate(logButtonPrefab, logListParent);
            newButton.GetComponentInChildren<TMP_Text>().text = log.title;

            Button btn = newButton.GetComponent<Button>();
            btn.interactable = true;
            btn.onClick.AddListener(() => ShowLog(log.content));
        }
    }

    public void ShowLog(string content)
    {
        logDisplayText.text = content;
        logDisplayPanel.SetActive(true);

        if (logScrollView != null) logScrollView.SetActive(false);
        if (backButton != null) backButton.SetActive(false);
        if (closeButton != null) closeButton.SetActive(true);
        if (logTabBar != null) logTabBar.SetActive(false); // ✅ Hide tab bar
    }

    public void SelectTutorialTab() => ChangeCategory(LogCategory.Tutorial);
    public void SelectJohnTab() => ChangeCategory(LogCategory.Character_John);
    public void SelectCynthiaTab() => ChangeCategory(LogCategory.Character_Cynthia);
    public void SelectStevenTab() => ChangeCategory(LogCategory.Character_Steven);

    public void CloseLogDisplay()
    {
        ResetLogView();
    }

    public void ResetLogView()
    {
        logDisplayPanel.SetActive(false);
        if (closeButton != null) closeButton.SetActive(false);
        if (logScrollView != null) logScrollView.SetActive(true);
        if (backButton != null) backButton.SetActive(true);
        if (logTabBar != null) logTabBar.SetActive(true); // ✅ Restore tab bar
    }

    public void Refresh()
    {
        RefreshLogList();
        UpdatePointsUI();
    }

    private void LoadUnlockedLogs()
    {
        foreach (var log in logEntries)
        {
            string key = $"LogUnlocked_{log.id}";
            log.unlocked = PlayerPrefs.GetInt(key, log.pointsRequired == 0 ? 1 : 0) == 1;
        }
    }

    public void SaveUnlockedLog(LogEntry log)
    {
        if (string.IsNullOrEmpty(log.id)) return;

        PlayerPrefs.SetInt($"LogUnlocked_{log.id}", 1);
        PlayerPrefs.Save();
    }
}
