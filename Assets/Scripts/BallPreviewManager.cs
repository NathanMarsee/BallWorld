using UnityEngine;
using UnityEngine.SceneManagement;

public class BallPreviewManager : MonoBehaviour
{
    public static BallPreviewManager Instance { get; private set; }

    public BallDatabase ballDatabase;
    public Transform previewAnchor;
    public float spinSpeed = 20f;

    private GameObject currentPreview;
    private int currentBallIndex = 0;
    private const string SelectedBallKey = "SelectedBall";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 🔥 Listen for scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        TryReconnectReferences();
        LoadSelectedBall();
        SpawnPreview();
    }

    void TryReconnectReferences()
    {
        if (previewAnchor == null)
        {
            var anchorObj = GameObject.Find("PreviewAnchor");
            if (anchorObj != null)
                previewAnchor = anchorObj.transform;
        }

        if (ballDatabase == null)
            ballDatabase = FindObjectOfType<BallDatabase>();
    }

    void LoadSelectedBall()
    {
        currentBallIndex = PlayerPrefs.GetInt(SelectedBallKey, 0);
    }

    void SpawnPreview()
    {
        if (previewAnchor == null || ballDatabase == null)
        {
            Debug.LogWarning("BallPreviewManager: Missing references!");
            return;
        }

        if (currentPreview != null)
            Destroy(currentPreview);

        var ball = ballDatabase.balls[currentBallIndex];
        if (ball.prefab == null)
        {
            Debug.LogWarning($"BallPreviewManager: Ball prefab missing for index {currentBallIndex}");
            return;
        }

        currentPreview = Instantiate(ball.prefab, previewAnchor.position, Quaternion.identity, previewAnchor);
        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localRotation = Quaternion.identity;
        currentPreview.name = $"PreviewBall_{ball.prefab.name}";
    }

    void Update()
    {
        if (currentPreview != null && SceneManager.GetActiveScene().name == "MainMenu")
        {
            currentPreview.transform.Rotate(Vector3.up * spinSpeed * Time.unscaledDeltaTime, Space.World);
        }
    }

    // 🔥 PUBLIC STATIC METHOD TO REFRESH PREVIEW
    public static void RefreshPreview()
    {
        if (Instance == null)
        {
            Debug.LogWarning("BallPreviewManager.Instance is missing!");
            return;
        }

        Instance.LoadSelectedBall();
        Instance.SpawnPreview();
    }

    // 🔥🔥 NEW: React to scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            TryReconnectReferences();

            // If the preview object exists but somehow lost its parent, fix it
            if (currentPreview != null && previewAnchor != null)
            {
                currentPreview.transform.SetParent(previewAnchor, false);
                currentPreview.transform.localPosition = Vector3.zero;
                currentPreview.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Clean up event listener
    }
}
