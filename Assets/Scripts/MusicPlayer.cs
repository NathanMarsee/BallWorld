using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{
    [Header("Playlist")]
    public AudioClip[] menuTracks;
    public AudioClip[] levelTracks;
    public AudioClip[] basketballTracks; // 🔥 NEW

    private AudioClip[] tracks;

    [Header("Settings")]
    public AudioSource audioSource;
    public bool loopPlaylist = true;
    public bool shuffle = false;

    private List<AudioClip> playlist = new List<AudioClip>();
    private int currentTrackIndex = 0;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        string currentScene = SceneManager.GetActiveScene().name;
        SelectPlaylistForScene(currentScene);

        BuildPlaylist();
        PlayTrack(currentTrackIndex);
    }

    void Update()
    {
        if (!audioSource.isPlaying && playlist.Count > 0)
        {
            PlayNextTrack();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SelectPlaylistForScene(scene.name);
    }

    void SelectPlaylistForScene(string sceneName)
    {
        if (sceneName == "Basketball")
        {
            SwapToPlaylist(basketballTracks);
        }
        else if (sceneName == "MainMenu")
        {
            SwapToPlaylist(menuTracks);
        }
        else
        {
            SwapToPlaylist(levelTracks);
        }
    }

    void SwapToPlaylist(AudioClip[] newTracks)
    {
        if (newTracks == null || newTracks.Length == 0)
        {
            Debug.LogWarning("MusicPlayer: Attempted to swap to an empty playlist.");
            return;
        }

        if (tracks == newTracks) return; // No need to reload same tracks

        audioSource.Stop();
        tracks = newTracks;
        BuildPlaylist();
        PlayTrack(0);
    }

    void BuildPlaylist()
    {
        playlist.Clear();
        playlist.AddRange(tracks);

        if (shuffle)
        {
            ShufflePlaylist();
        }

        currentTrackIndex = 0;
    }

    void ShufflePlaylist()
    {
        for (int i = 0; i < playlist.Count - 1; i++)
        {
            int j = Random.Range(i, playlist.Count);
            var temp = playlist[i];
            playlist[i] = playlist[j];
            playlist[j] = temp;
        }

        if (playlist.Count > 1 && playlist[0] == tracks[currentTrackIndex])
        {
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
                    ShufflePlaylist();

                currentTrackIndex = 0;
            }
            else
            {
                return;
            }
        }

        PlayTrack(currentTrackIndex);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
