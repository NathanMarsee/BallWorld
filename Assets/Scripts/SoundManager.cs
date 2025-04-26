using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioClip pointResetSound;
    public AudioClip levelResetSound;
    public AudioClip pointSound;
    public AudioClip unlockSound;
    public AudioClip errorSound; // 🔥 NEW - Error sound

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(transform.root.gameObject);
            }
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayPointSound()
    {
        audioSource.PlayOneShot(pointSound);
    }

    public void PlayPointResetSound()
    {
        audioSource.PlayOneShot(pointResetSound);
    }

    public void PlayLevelResetSound()
    {
        audioSource.PlayOneShot(levelResetSound);
    }

    public void PlayUnlockSound()
    {
        audioSource.PlayOneShot(unlockSound);
    }

    public void PlayErrorSound() // 🔥 NEW
    {
        if (errorSound != null)
        {
            audioSource.PlayOneShot(errorSound);
        }
        else
        {
            Debug.LogWarning("SoundManager: No errorSound assigned.");
        }
    }
}
