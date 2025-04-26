using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class BootstrapManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject notificationSystemPrefab;
    public GameObject pauseMenuPrefab;

    private bool lastInputWasGamepad = false;
    private float inputCheckCooldown = 0f;

    void Awake()
    {
        // 🔥 Bootstrap Notification System
        if (FindObjectOfType<NotificationManager>() == null && notificationSystemPrefab != null)
        {
            GameObject notif = Instantiate(notificationSystemPrefab);
            DontDestroyOnLoad(notif);
        }

        // 🔥 Bootstrap Pause Menu
        if (FindObjectOfType<MenuManager>() == null && pauseMenuPrefab != null)
        {
            GameObject pauseMenu = Instantiate(pauseMenuPrefab);
            DontDestroyOnLoad(pauseMenu);
        }
    }

    void Update()
    {
        inputCheckCooldown -= Time.unscaledDeltaTime;

        if (inputCheckCooldown > 0f) return; // Prevent checking every frame too fast

        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            if (!lastInputWasGamepad)
            {
                ActivateControllerUI();
                lastInputWasGamepad = true;
            }
            inputCheckCooldown = 0.1f;
        }
        else if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (lastInputWasGamepad)
            {
                DeactivateControllerUI();
                lastInputWasGamepad = false;
            }
            inputCheckCooldown = 0.1f;
        }
        else if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        {
            if (lastInputWasGamepad)
            {
                DeactivateControllerUI();
                lastInputWasGamepad = false;
            }
            inputCheckCooldown = 0.1f;
        }
    }

    private void ActivateControllerUI()
    {
        if (EventSystem.current == null) return;

        Button bestButton = FindBestButton();
        if (bestButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(bestButton.gameObject);
        }
    }

    private void DeactivateControllerUI()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private Button FindBestButton()
    {
        var buttons = GameObject.FindObjectsOfType<Button>(true)
            .Where(b => b.gameObject.activeInHierarchy && b.interactable)
            .ToList();

        if (buttons.Count == 0)
            return null;

        return buttons
            .OrderByDescending(b => b.transform.position.y)
            .ThenBy(b => b.transform.position.x)
            .FirstOrDefault();
    }
}
