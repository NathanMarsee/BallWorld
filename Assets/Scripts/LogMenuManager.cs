using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LogMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject logButtonPrefab;
    public Transform logListParent;
    public GameObject logDisplayPanel;
    public TMP_Text logDisplayText;
    public TMP_Text pointsText;
    public GameObject closeButton; // ✅ Added close button reference

    [Header("Background Elements to Disable")]
    public GameObject logScrollView;
    public GameObject backButton;

    [Header("Log Data")]
    public List<LogEntry> logEntries;

    private int playerPoints = 0;

    void Start()
    {
        ResetLogView(); // ✅ Ensure clean state
        RefreshLogList();
        UpdatePointsUI();
    }

    public void SetPlayerPoints(int points)
    {
        playerPoints = points;
        RefreshLogList();
        UpdatePointsUI();
    }

    void UpdatePointsUI()
    {
        pointsText.text = $"Points: {playerPoints}";
    }

    void RefreshLogList()
    {
        foreach (Transform child in logListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var log in logEntries)
        {
            GameObject newButton = Instantiate(logButtonPrefab, logListParent);
            newButton.GetComponentInChildren<TMP_Text>().text = log.title;

            Button btn = newButton.GetComponent<Button>();

            if (playerPoints >= log.pointsRequired)
            {
                btn.interactable = true;
                btn.onClick.AddListener(() => ShowLog(log.content));
            }
            else
            {
                btn.interactable = false;
                newButton.GetComponentInChildren<TMP_Text>().text += $" (Locked)";
            }
        }
    }

    public void ShowLog(string content)
    {
        logDisplayText.text = content;
        logDisplayPanel.SetActive(true);

        // Hide scroll view and back button
        if (logScrollView != null) logScrollView.SetActive(false);
        if (backButton != null) backButton.SetActive(false);
        if (closeButton != null) closeButton.SetActive(true);
    }

    public void CloseLogDisplay()
    {
        ResetLogView();
    }

    // ✅ Call this every time LogsPanel is activated
    public void ResetLogView()
    {
        logDisplayPanel.SetActive(false);
        if (closeButton != null) closeButton.SetActive(false);
        if (logScrollView != null) logScrollView.SetActive(true);
        if (backButton != null) backButton.SetActive(true);
    }
}
