using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TutorialPopupController : MonoBehaviour
{
    [Header("Popup Elements")]
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button closeButton;

    private PlayerControls controls;
    private bool isPopupActive = false;
    private string sceneSeenKey;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.UI.Enable();
        controls.UI.Cancel.performed += OnCancelPressed;
        controls.UI.Submit.performed += OnSubmitPressed;
    }

    private void OnDisable()
    {
        controls.UI.Cancel.performed -= OnCancelPressed;
        controls.UI.Submit.performed -= OnSubmitPressed;
        controls.UI.Disable();
    }

    void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePopup);

        sceneSeenKey = "TutorialSeen_" + SceneManager.GetActiveScene().name;

        // Check if player already saw it
        if (PlayerPrefs.GetInt(sceneSeenKey, 0) == 0)
        {
            OpenPopup();
            PlayerPrefs.SetInt(sceneSeenKey, 1);
            PlayerPrefs.Save();
        }
    }

    private void OpenPopup()
    {
        if (popupPanel == null)
            return;

        popupPanel.SetActive(true);
        Time.timeScale = 0f;
        isPopupActive = true;

        EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        Time.timeScale = 1.2f;
        isPopupActive = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (isPopupActive)
        {
            ClosePopup();
        }
    }

    private void OnSubmitPressed(InputAction.CallbackContext context)
    {
        if (isPopupActive && EventSystem.current.currentSelectedGameObject == closeButton.gameObject)
        {
            ClosePopup();
        }
    }
}
