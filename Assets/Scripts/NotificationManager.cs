using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    public GameObject notificationPrefab;
    public GameObject notificationSystemPrefab;
    public Transform notificationContainer;

    private int pendingPoints = 0;
    private Coroutine notificationRoutine;
    private float bufferTime = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for general-purpose notifications
    public void ShowNotification(string message, float duration = 8f)
    {
        GameObject notification = Instantiate(notificationPrefab, notificationContainer);
        TMP_Text text = notification.GetComponentInChildren<TMP_Text>();
        text.text = message;

        StartCoroutine(FadeAndDestroy(notification, duration));
    }

    // Use this to queue point gain notifications
    public void QueuePointNotification(int pointsToAdd)
    {
        pendingPoints += pointsToAdd;

        if (notificationRoutine == null)
            notificationRoutine = StartCoroutine(ShowBufferedPoints());
    }

    private IEnumerator ShowBufferedPoints()
    {
        yield return new WaitForSeconds(bufferTime);

        string message = $"+{pendingPoints} Points!";
        ShowNotification(message);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayPointSound();
        }

        pendingPoints = 0;
        notificationRoutine = null;
    }

    private IEnumerator FadeAndDestroy(GameObject obj, float duration)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        yield return new WaitForSeconds(duration - 1f);

        float fadeDuration = 1f;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        Destroy(obj);
    }
}
