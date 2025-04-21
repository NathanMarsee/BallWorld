using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle fullscreenToggle;

    public AudioMixer audioMixer; // ← Drag your MainMixer here in Inspector

    void Start()
    {
        // Load saved settings or use defaults
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        ApplySettings();
    }

    public void ApplySettings()
    {
        // Set exposed mixer parameters
        SetMixerVolume("MasterVolume", masterVolumeSlider.value);
        SetMixerVolume("MusicVolume", musicVolumeSlider.value);
        SetMixerVolume("SFXVolume", sfxVolumeSlider.value);

        // Apply fullscreen
        Screen.fullScreen = fullscreenToggle.isOn;

        // Save preferences
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetMixerVolume(string exposedParam, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }
}
