using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Playlist")]
    public AudioClip[] tracks;

    [Header("Settings")]
    public AudioSource audioSource;
    public bool loopPlaylist = true;

    private int currentTrackIndex = 0;

    void Start()
    {
        if (tracks.Length > 0 && audioSource != null)
        {
            audioSource.clip = tracks[currentTrackIndex];
            audioSource.Play();
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        if (tracks.Length == 0) return;

        currentTrackIndex++;

        if (currentTrackIndex >= tracks.Length)
        {
            if (loopPlaylist)
                currentTrackIndex = 0;
            else
                return; // Done playing
        }

        audioSource.clip = tracks[currentTrackIndex];
        audioSource.Play();
    }
}
