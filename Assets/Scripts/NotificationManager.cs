using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    public GameObject notificationPrefab;
    public GameObject notificationSystemPrefab;
    public Transform notificationContainer;

    // Inside NotificationManager.cs
void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }
    else
    {
        Destroy(gameObject); // prevent duplicates across scenes
    }
}




    public void ShowNotification(string message, float duration = 8f)
    {
        GameObject notification = Instantiate(notificationPrefab, notificationContainer);
        TMP_Text text = notification.GetComponentInChildren<TMP_Text>();
        text.text = message;

        StartCoroutine(FadeAndDestroy(notification, duration));
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
