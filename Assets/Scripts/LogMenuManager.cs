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
    public GameObject logsHeader;

    public TMP_Text pointsText;
    public GameObject closeButton;
    public GameObject nextPageButton;
    public GameObject prevPageButton;
    public GameObject logTabBar;

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

    private string[] currentPages;
    private int currentPageIndex = 0;

    void OnEnable()
    {
        Debug.Log("[LogMenuManager] OnEnable called.");

        pointManager = PointManager.Instance ?? FindObjectOfType<PointManager>();

        if (pointManager == null)
        {
            Debug.LogError("[LogMenuManager] No PointManager found in scene.");
            return;
        }

        SetupTabButtons();
        LoadUnlockedLogs();
        ResetLogView();
        RefreshLogList();
        UpdatePointsUI();
    }

    void SetupTabButtons()
    {
        Debug.Log("[LogMenuManager] Setting up tab buttons.");

        tutorialTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Tutorial));
        johnTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Character_John));
        cynthiaTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Character_Cynthia));
        stevenTabButton.onClick.AddListener(() => ChangeCategory(LogCategory.Character_Steven));
    }

    void ChangeCategory(LogCategory category)
    {
        Debug.Log($"[LogMenuManager] Changing category to {category}.");
        currentCategory = category;
        RefreshLogList();
    }

    void UpdatePointsUI()
    {
        if (pointManager != null)
        {
            Debug.Log($"[LogMenuManager] Updating points UI: {pointManager.CurrentPoints} points.");
            pointsText.text = $"Points: {pointManager.CurrentPoints}";
        }
    }

    public void RefreshLogList()
    {
        Debug.Log("[LogMenuManager] Refreshing log list.");

        foreach (Transform child in logListParent)
            Destroy(child.gameObject);

        var filteredLogs = logEntries
            .Where(log => log.category == currentCategory && log.unlocked);

        Debug.Log($"[LogMenuManager] Found {filteredLogs.Count()} logs for category {currentCategory}.");

        foreach (var log in filteredLogs)
        {
            GameObject newButton = Instantiate(logButtonPrefab, logListParent);
            newButton.GetComponentInChildren<TMP_Text>().text = log.title;

            Button btn = newButton.GetComponent<Button>();
            btn.interactable = true;
            btn.onClick.AddListener(() => ShowLog(log.content));

            Debug.Log($"[LogMenuManager] Created button for log: {log.title}");
        }
    }

    public void ShowLog(string content)
    {
        Debug.Log("[LogMenuManager] Showing a log.");

        currentPages = content.Split(new[] { "[PAGE]" }, System.StringSplitOptions.None);
        currentPageIndex = 0;

        DisplayCurrentPage();

        logDisplayPanel.SetActive(true);
        logsHeader?.SetActive(false);
        logScrollView?.SetActive(false);
        backButton?.SetActive(false);
        closeButton?.SetActive(true);
        logTabBar?.SetActive(false);

        Debug.Log("[LogMenuManager] Log display panel activated.");

        FindObjectOfType<MenuManager>()?.SetFirstLogEntryButton(); // Controller focus
    }

    void DisplayCurrentPage()
    {
        if (currentPages != null && currentPageIndex >= 0 && currentPageIndex < currentPages.Length)
        {
            Debug.Log($"[LogMenuManager] Displaying page {currentPageIndex + 1}/{currentPages.Length}");
            logDisplayText.text = currentPages[currentPageIndex].Trim();
        }

        prevPageButton?.SetActive(currentPageIndex > 0);
        nextPageButton?.SetActive(currentPages != null && currentPageIndex < currentPages.Length - 1);
    }

    public void NextPage()
    {
        if (currentPages != null && currentPageIndex < currentPages.Length - 1)
        {
            currentPageIndex++;
            DisplayCurrentPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            DisplayCurrentPage();
        }
    }

    public void CloseLogDisplay()
    {
        Debug.Log("[LogMenuManager] Closing log display.");
        ResetLogView();
    }

    public void ResetLogView()
    {
        Debug.Log("[LogMenuManager] Resetting log view.");

        logDisplayPanel.SetActive(false);
        closeButton?.SetActive(false);
        nextPageButton?.SetActive(false);
        prevPageButton?.SetActive(false);
        logScrollView?.SetActive(true);
        backButton?.SetActive(true);
        logTabBar?.SetActive(true);
        logsHeader?.SetActive(true);
    }

    public void Refresh()
    {
        Debug.Log("[LogMenuManager] Refreshing all.");

        LoadUnlockedLogs();
        RefreshLogList();
        UpdatePointsUI();
    }

    private void LoadUnlockedLogs()
    {
        Debug.Log("[LogMenuManager] Loading unlocked logs.");

        foreach (var log in logEntries)
        {
            string key = $"LogUnlocked_{log.id}";
            log.unlocked = PlayerPrefs.GetInt(key, log.pointsRequired == 0 ? 1 : 0) == 1;
        }
    }

    public void SaveUnlockedLog(LogEntry log)
    {
        if (string.IsNullOrEmpty(log.id)) return;

        Debug.Log($"[LogMenuManager] Saving unlocked log: {log.title}");
        PlayerPrefs.SetInt($"LogUnlocked_{log.id}", 1);
        PlayerPrefs.Save();
    }
}
