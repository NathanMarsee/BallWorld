using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BallDropdownHandler : MonoBehaviour
{
    [Header("References")]
    public TMP_Dropdown ballDropdown;
    public BallDatabase ballDatabase;
    public Transform previewAnchor; // 🧷 Drop a world-space empty object here in Inspector
    public float previewScale = 100f;
    public float spinSpeed = 20f;

    private GameObject currentPreview;

    void Start()
    {
        if (ballDropdown == null || ballDatabase == null || previewAnchor == null)
        {
            Debug.LogError("BallDropdownHandler: Missing references. Check Inspector.");
            return;
        }

        PopulateDropdown();

        if (PlayerPrefs.HasKey("SelectedBall"))
        {
            int savedIndex = PlayerPrefs.GetInt("SelectedBall");
            ballDropdown.value = savedIndex;
            BallSelector.Instance?.SelectBall(savedIndex);
            ShowPreview(savedIndex);
        }

        ballDropdown.onValueChanged.AddListener(OnBallSelected);
    }

    void PopulateDropdown()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (GameObject prefab in ballDatabase.ballPrefabs)
        {
            string label = prefab != null ? prefab.name : "Unnamed Ball";
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
        ShowPreview(index);
    }

    void ShowPreview(int index)
    {
        if (currentPreview != null)
            Destroy(currentPreview);

        GameObject prefab = ballDatabase.ballPrefabs[index];
        if (prefab == null)
        {
            Debug.LogWarning($"BallDropdownHandler: Missing prefab at index {index}");
            return;
        }

        currentPreview = Instantiate(prefab, previewAnchor.position, Quaternion.identity);
        currentPreview.transform.localScale = Vector3.one * previewScale;
        currentPreview.transform.rotation = previewAnchor.rotation;
        currentPreview.name = $"PreviewBall_{prefab.name}";
        Debug.Log($"BallDropdownHandler: Spawned preview of {prefab.name} at {previewAnchor.position}");
    }

    void Update()
    {
        if (currentPreview != null)
            currentPreview.transform.Rotate(Vector3.up * spinSpeed * Time.unscaledDeltaTime, Space.World);
    }

    void OnDisable()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }
}
