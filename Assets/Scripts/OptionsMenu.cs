using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Options Menu Controller
/// Handles audio volume and camera sensitivity settings.
/// Saves preferences to PlayerPrefs so they persist between sessions.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("Slider for controlling master volume")]
    public Slider volumeSlider;

    [Tooltip("Slider for controlling camera sensitivity")]
    public Slider sensitivitySlider;

    [Header("Default Settings")]
    [Tooltip("Default volume (0.0 to 1.0)")]
    public float defaultVolume = 1.0f;

    [Tooltip("Default sensitivity")]
    public float defaultSensitivity = 2.0f;

    // Keys for PlayerPrefs storage
    private const string PREF_VOLUME = "MasterVolume";
    private const string PREF_SENSITIVITY = "CameraSensitivity";

    private void Start()
    {
        InitializeSettings();
    }

    /// <summary>
    /// Load saved settings and setup UI listeners
    /// </summary>
    private void InitializeSettings()
    {
        // 1. Load Volume
        float savedVolume = PlayerPrefs.GetFloat(PREF_VOLUME, defaultVolume);
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            // Remove existing listeners to avoid duplicates if re-enabled
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        AudioListener.volume = savedVolume;

        // 2. Load Sensitivity
        float savedSensitivity = PlayerPrefs.GetFloat(PREF_SENSITIVITY, defaultSensitivity);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSensitivity;
            // Remove existing listeners to avoid duplicates if re-enabled
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
    }

    /// <summary>
    /// Set global volume and save preference
    /// </summary>
    /// <param name="volume">Volume level (0.0 to 1.0)</param>
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(PREF_VOLUME, volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Set camera sensitivity and save preference.
    /// Also attempts to update the controller immediately if found (for Pause Menus).
    /// </summary>
    /// <param name="sensitivity">Sensitivity value</param>
    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(PREF_SENSITIVITY, sensitivity);
        PlayerPrefs.Save();

        // Optional: If we are in-game and the player exists, update it immediately
        ThirdPersonController player = FindObjectOfType<ThirdPersonController>();
        if (player != null)
        {
            player.cameraSensitivity = sensitivity;
        }
    }
}
