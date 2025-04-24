using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Goal: MonoBehaviour
{
    // Public Variables (set in the Inspector)
    [SerializeField] private GameObject object1ToDestroy; // First object to be destroyed
    [SerializeField] private GameObject object2ToDestroy; // Second object to be destroyed
    [SerializeField] private GameObject textScreen;       // The UI Text screen GameObject
    [SerializeField] private Button returnButton;         // The button to return to the main menu
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Name of the main menu scene
    public float destroyDelay = 0f;
    public bool returnOnSpacebar = true; // Added option to return on spacebar

    private bool object1Destroyed = false;
    private bool object2Destroyed = false;
    private bool goalAchieved = false;
    private bool canvasActive = false; // Track if the canvas is active

    void Start()
    {
        // Ensure the text screen and button are initially inactive
        if (textScreen != null)
            textScreen.SetActive(false);
        if (returnButton != null)
            returnButton.gameObject.SetActive(false);

        // Subscribe to the OnDestroy event for each target object.
        if (object1ToDestroy != null)
        {
            ObjectDestroyedListener destroyer1 = object1ToDestroy.AddComponent<ObjectDestroyedListener>();
            destroyer1.OnDestroyed += HandleObject1Destroyed;
        }
        if (object2ToDestroy != null)
        {
            ObjectDestroyedListener destroyer2 = object2ToDestroy.AddComponent<ObjectDestroyedListener>();
            destroyer2.OnDestroyed += HandleObject2Destroyed;
        }


        // Add a listener to the button's onClick event
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnToMainMenu);
        }
        else
        {
            UnityEngine.Debug.LogError("GoalTextAndMenu: Return button is not assigned in the inspector, returning to main menu will not work");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events.
        if (object1ToDestroy != null)
        {
            ObjectDestroyedListener destroyer1 = object1ToDestroy.GetComponent<ObjectDestroyedListener>();
            if (destroyer1 != null)
            {
                destroyer1.OnDestroyed -= HandleObject1Destroyed;
            }
        }
        if (object2ToDestroy != null)
        {
            ObjectDestroyedListener destroyer2 = object2ToDestroy.GetComponent<ObjectDestroyedListener>();
            if (destroyer2 != null)
            {
                destroyer2.OnDestroyed -= HandleObject2Destroyed;
            }
        }
    }

    void Update()
    {
        // Check for space bar input, but only if the canvas is active and returnOnSpacebar is true
        if (returnOnSpacebar && canvasActive && Input.GetKeyDown(KeyCode.Space))
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
            UnityEngine.Debug.Log("Goal Achieved: Both objects destroyed.");
            // Activate the text screen and button
            if (textScreen != null)
            {
                textScreen.SetActive(true);
                canvasActive = true; // Set the flag when the canvas is activated
            }
            if (returnButton != null)
                returnButton.gameObject.SetActive(true);
        }
    }

    void ReturnToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
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