using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Goal : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private GameObject object1ToDestroy;
    [SerializeField] private GameObject object2ToDestroy;

    [Header("UI Elements")]
    [SerializeField] private GameObject textScreen;
    [SerializeField] private Button returnButton;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool object1Destroyed = false;
    private bool object2Destroyed = false;
    private bool goalAchieved = false;
    private bool canvasActive = false;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void Start()
    {
        // Hide text and button at start
        if (textScreen != null)
            textScreen.SetActive(false);
        if (returnButton != null)
            returnButton.gameObject.SetActive(false);

        // Setup object listeners
        if (object1ToDestroy != null)
        {
            var destroyer1 = object1ToDestroy.AddComponent<ObjectDestroyedListener>();
            destroyer1.OnDestroyed += HandleObject1Destroyed;
        }

        if (object2ToDestroy != null)
        {
            var destroyer2 = object2ToDestroy.AddComponent<ObjectDestroyedListener>();
            destroyer2.OnDestroyed += HandleObject2Destroyed;
        }

        if (returnButton != null)
            returnButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.UI.Submit.performed += OnSubmitPressed;
    }

    private void OnDisable()
    {
        controls.UI.Submit.performed -= OnSubmitPressed;
        controls.Disable();
    }

    private void OnSubmitPressed(InputAction.CallbackContext context)
    {
        if (canvasActive)
        {
            ReturnToMainMenu();
        }
    }

    private void HandleObject1Destroyed()
    {
        object1Destroyed = true;
        CheckForGoalAchieved();
    }

    private void HandleObject2Destroyed()
    {
        object2Destroyed = true;
        CheckForGoalAchieved();
    }

    void CheckForGoalAchieved()
    {
        if (object1Destroyed && object2Destroyed && !goalAchieved)
        {
            goalAchieved = true;
            if (textScreen != null)
            {
                textScreen.SetActive(true);
                canvasActive = true;
            }
            if (returnButton != null)
                returnButton.gameObject.SetActive(true);
        }
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    // This component listens for the OnDestroy event.
public class ObjectDestroyedListener : MonoBehaviour
{
    public delegate void OnDestroyedEventHandler();
    public event OnDestroyedEventHandler OnDestroyed;

    private void OnDestroy()
    {
        if (OnDestroyed != null)
        {
            OnDestroyed();
        }
    }
}

}
