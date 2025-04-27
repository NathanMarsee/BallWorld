using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{
    [Header("Playlists")]
    public AudioClip[] menuTracks;
    public AudioClip[] levelTracks;
    public AudioClip[] basketballTracks;
    public AudioClip[] testTracks; // 🔥 Used for Extraction scene now

    private AudioClip[] currentTrackList;

    [Header("Settings")]
    public AudioSource audioSource;
    public bool loopPlaylist = true;
    public bool shuffle = false;

    private List<AudioClip> playlist = new List<AudioClip>();
    private int currentTrackIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 🔥 optional if you want music across scenes
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        string currentScene = SceneManager.GetActiveScene().name;
        SelectPlaylistForScene(currentScene);
        BuildPlaylist();
        PlayTrack(currentTrackIndex);
    }

    private void Update()
    {
        if (!audioSource.isPlaying && playlist.Count > 0)
        {
            if (currentTrackIndex < playlist.Count)
                PlayNextTrack();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SelectPlaylistForScene(scene.name);
    }

    private void SelectPlaylistForScene(string sceneName)
    {
        if (sceneName == "Basketball")
        {
            SwapToPlaylist(basketballTracks);
        }
        else if (sceneName == "MainMenu")
        {
            SwapToPlaylist(menuTracks);
        }
        else if (sceneName == "Extraction") // 🔥 Corrected here
        {
            SwapToPlaylist(testTracks);
        }
        else if (sceneName == "Infinite" || sceneName == "Tutorial")
        {
            SwapToPlaylist(levelTracks);
        }
        else
        {
            SwapToPlaylist(levelTracks); // fallback
        }
    }

    private void SwapToPlaylist(AudioClip[] newTracks)
    {
        if (newTracks == null || newTracks.Length == 0)
        {
            Debug.LogWarning("MusicPlayer: Attempted to swap to an empty playlist.");
            return;
        }

        if (currentTrackList == newTracks)
            return; // already using correct playlist

        audioSource.Stop();
        currentTrackList = newTracks;
        BuildPlaylist();
        PlayTrack(0);
    }

    private void BuildPlaylist()
    {
        playlist.Clear();
        playlist.AddRange(currentTrackList);

        if (shuffle)
            ShufflePlaylist();

        currentTrackIndex = 0;
    }

    private void ShufflePlaylist()
    {
        for (int i = 0; i < playlist.Count; i++)
        {
            int j = Random.Range(i, playlist.Count);
            (playlist[i], playlist[j]) = (playlist[j], playlist[i]);
        }
    }

    private void PlayTrack(int index)
    {
        if (index < 0 || index >= playlist.Count)
        {
            Debug.LogWarning($"MusicPlayer: Invalid track index {index}");
            return;
        }

        audioSource.clip = playlist[index];
        audioSource.Play();
    }

    private void PlayNextTrack()
    {
        currentTrackIndex++;

        if (currentTrackIndex >= playlist.Count)
        {
            if (loopPlaylist)
            {
                if (shuffle)
                    ShufflePlaylist();

                currentTrackIndex = 0;
                PlayTrack(currentTrackIndex);
            }
            else
            {
                audioSource.Stop();
            }
        }
        else
        {
            PlayTrack(currentTrackIndex);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
