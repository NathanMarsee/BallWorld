using UnityEngine;
using TMPro;

public class DifficultyDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        if (dropdown == null)
            dropdown = GetComponent<TMP_Dropdown>();

        dropdown.SetValueWithoutNotify(DifficultyManager.Instance.GetSavedDifficultyIndex());
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        DifficultyManager.Instance.SetDifficultyFromDropdown(index);
    }
}
