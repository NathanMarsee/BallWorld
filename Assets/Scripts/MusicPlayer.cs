using UnityEngine;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{
    [Header("Playlist")]
    public AudioClip[] menuTracks;
    public AudioClip[] levelTracks;
    private AudioClip[] tracks;

    [Header("Settings")]
    public AudioSource audioSource;
    public bool loopPlaylist = true;
    public bool shuffle = false;

    private List<AudioClip> playlist = new List<AudioClip>();
    private int currentTrackIndex = 0;

    void Start()
    {
        tracks = menuTracks;
        if (tracks.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("MusicPlayer: No tracks or AudioSource assigned.");
            return;
        }
        BuildPlaylist();
        PlayTrack(currentTrackIndex);
    }

    public void SwapToLevelPlaylist()
    {
        audioSource.Stop();
        tracks = levelTracks;
        BuildPlaylist();
        audioSource.Stop();
    }
    public void SwapToMenuPlaylist()
    {
        audioSource.Stop();
        tracks = menuTracks;
        BuildPlaylist();
        audioSource.Stop();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void BuildPlaylist()
    {
        playlist.Clear();
        playlist.AddRange(tracks);

        if (shuffle)
        {
            ShufflePlaylist();
        }
    }

    void ShufflePlaylist()
    {
        // Fisher-Yates Shuffle
        for (int i = 0; i < playlist.Count - 1; i++)
        {
            int j = Random.Range(i, playlist.Count);
            var temp = playlist[i];
            playlist[i] = playlist[j];
            playlist[j] = temp;
        }

        // Prevent starting with the same song twice in a row if possible
        if (playlist.Count > 1 && playlist[0] == tracks[currentTrackIndex])
        {
            // Swap with another random song
            int swapIndex = Random.Range(1, playlist.Count);
            var temp = playlist[0];
            playlist[0] = playlist[swapIndex];
            playlist[swapIndex] = temp;
        }

        currentTrackIndex = 0;
    }

    void PlayTrack(int index)
    {
        if (index < 0 || index >= playlist.Count) return;

        audioSource.clip = playlist[index];
        audioSource.Play();
    }

    void PlayNextTrack()
    {
        currentTrackIndex++;

        if (currentTrackIndex >= playlist.Count)
        {
            if (loopPlaylist)
            {
                if (shuffle)
                    ShufflePlaylist(); // Re-shuffle between loops

                currentTrackIndex = 0;
            }
            else
            {
                return;
            }
        }

        PlayTrack(currentTrackIndex);
    }
}
