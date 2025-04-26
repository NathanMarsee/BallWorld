using UnityEngine;
using UnityEngine.SceneManagement;

public class BallSelector : MonoBehaviour
{
    public static BallSelector Instance { get; private set; }

    [Header("Assign your database")]
    public BallDatabase database;

    [Header("Default Selection")]
    public int selectedBallIndex = 0;

    private const string SelectedBallKey = "SelectedBallIndex";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadSelectedBall();
    }

    public void SelectBall(int index)
    {
        if (database == null || database.balls == null || database.balls.Length == 0)
        {
            Debug.LogError("BallSelector: Database not set up correctly!");
            return;
        }

        selectedBallIndex = Mathf.Clamp(index, 0, database.balls.Length - 1);
        SaveSelectedBall();
        ApplySelectionNow();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "Tutorial") return;
        ApplySelectionNow();
    }

    private void ReplaceBallInScene(string sceneName)
    {
        GameObject existingBall = GameObject.FindGameObjectWithTag("Ball");

        if (existingBall == null)
        {
            Debug.LogWarning("BallSelector: No GameObject with tag 'Ball' found.");
            return;
        }

        var prefab = database?.balls[selectedBallIndex]?.prefab;

        if (prefab == null)
        {
            Debug.LogWarning($"BallSelector: Prefab at index {selectedBallIndex} missing or database missing.");
            return;
        }

        // Save current ball transform info
        Transform parent = existingBall.transform.parent;
        Vector3 localPos = existingBall.transform.localPosition;
        Quaternion localRot = existingBall.transform.localRotation;
        Vector3 localScale = existingBall.transform.localScale;

        Destroy(existingBall);

        // Instantiate new ball
        GameObject newBall = Instantiate(prefab);
        newBall.transform.SetParent(parent, false);
        newBall.transform.localPosition = localPos;
        newBall.transform.localRotation = localRot;
        newBall.transform.localScale = localScale;

        newBall.tag = "Ball";
        newBall.name = newBall.name.Replace("(Clone)", "");
    }

    public void ApplySelectionNow()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMenu" || sceneName == "Tutorial") return;

        ReplaceBallInScene(sceneName);
    }

    private void SaveSelectedBall()
    {
        PlayerPrefs.SetInt(SelectedBallKey, selectedBallIndex);
        PlayerPrefs.Save();
    }

    private void LoadSelectedBall()
    {
        selectedBallIndex = PlayerPrefs.GetInt(SelectedBallKey, 0);
    }
}
