using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Win Zone Controller
/// Detects when the player enters the zone, shows a Win UI, and handles level transition.
/// </summary>
public class WinZone : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("The UI Panel to show when the player wins")]
    public GameObject winScreenPanel;

    [Header("Level Settings")]
    [Tooltip("Name of the next level scene to load")]
    public string nextLevelSceneName = "Level2";

    [Tooltip("Enable this if you want to be sent to Main Menu instead of a next level")]
    public bool isFinalLevel = false;

    // Internal state
    private bool levelCompleted = false;

    private void Start()
    {
        // Ensure the win screen is hidden at start
        if (winScreenPanel != null)
        {
            winScreenPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[WinZone] Object entered trigger: {other.gameObject.name} with Tag: {other.tag}");

        // Only trigger if we haven't already won, and the player touched us
        if (!levelCompleted)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("[WinZone] Player tag detected! Winning level...");
                CompleteLevel();
            }
            else
            {
                Debug.LogWarning($"[WinZone] Object '{other.gameObject.name}' entered, but its tag is '{other.tag}', not 'Player'. Please change the Player object's tag to 'Player'.");
            }
        }
    }

    /// <summary>
    /// Handle the level completion logic
    /// </summary>
    private void CompleteLevel()
    {
        Debug.Log("Level Completed!");
        levelCompleted = true;

        // Show Win Screen
        if (winScreenPanel != null)
        {
            winScreenPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("WinScreenPanel is not assigned! Loading next level immediately...");
            LoadNextLevel();
            return;
        }

        // Pause the game mechanics
        Time.timeScale = 0f;

        // Unlock cursor so player can click "Next Level" button
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Called by the "Next Level" button on the UI
    /// </summary>
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Always unpause before changing scenes

        if (isFinalLevel)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            if (Application.CanStreamedLevelBeLoaded(nextLevelSceneName))
            {
                SceneManager.LoadScene(nextLevelSceneName);
            }
            else
            {
                Debug.LogError($"Scene '{nextLevelSceneName}' not found! Check Build Settings.");
            }
        }
    }

    /// <summary>
    /// Called by the "Menu" button on the UI
    /// </summary>
    public void LoadMainMenu()
    {
        Debug.Log("[WinZone] Loading Main Menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Alias for LoadMainMenu, use this for the "Quit" button if preferred
    /// </summary>
    public void QuitToMainMenu()
    {
        LoadMainMenu();
    }
}
