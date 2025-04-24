using UnityEngine;
using UnityEngine.SceneManagement;

public class BallSelector : MonoBehaviour
{
    public static BallSelector Instance { get; private set; }

    [Header("Assign your database")]
    public BallDatabase database;

    [Header("Default Selection")]
    public int selectedBallIndex = 0;

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
    }

    public void SelectBall(int index)
    {
        selectedBallIndex = Mathf.Clamp(index, 0, database.ballPrefabs.Length - 1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 🛑 Skip ball replacement in MainMenu and Tutorial scenes
        if (scene.name == "MainMenu" || scene.name == "Tutorial") return;

        ReplaceBallInScene(scene.name);
    }

    private void ReplaceBallInScene(string sceneName)
    {
        GameObject existingBall = GameObject.FindGameObjectWithTag("Ball");

        if (existingBall == null)
        {
            Debug.LogWarning("BallSelector: No GameObject with tag 'Ball' found.");
            return;
        }

        Transform parent = existingBall.transform.parent;
        Vector3 localPos = existingBall.transform.localPosition;
        Quaternion localRot = existingBall.transform.localRotation;
        Vector3 localScale = existingBall.transform.localScale;

        Destroy(existingBall);

        GameObject newBall = Instantiate(database.ballPrefabs[selectedBallIndex]);
        newBall.transform.SetParent(parent, worldPositionStays: false);
        newBall.transform.localPosition = localPos;
        newBall.transform.localRotation = localRot;
        newBall.transform.localScale = localScale;

        newBall.tag = "Ball";
        newBall.name = newBall.name.Replace("(Clone)", "");
    }
}
