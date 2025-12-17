using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause Menu Controller
/// Handles pausing the game, showing the menu, and scene transitions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The Panel containing the Pause Menu buttons")]
    public GameObject pauseMenuUI;

    [Tooltip("The Panel containing the Options (optional)")]
    public GameObject optionsMenuUI;

    [Header("Settings")]
    [Tooltip("Name of the Main Menu scene")]
    public string mainMenuSceneName = "MainMenu";

    // Track state
    public static bool GameIsPaused = false;

    void Update()
    {
        // Listen for the Pause key (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[PauseMenu] Escape key pressed!");
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void Resume()
    {
        Debug.Log("[PauseMenu] Resuming Game");
        // Close menus
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);

        // Resume time
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Lock cursor again for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    void Pause()
    {
        Debug.Log("[PauseMenu] Pausing Game");
        
        if (pauseMenuUI == null)
        {
            Debug.LogError("[PauseMenu] CRITICAL: 'Pause Menu UI' is not assigned in Inspector!");
            return;
        }

        // Open pause menu
        pauseMenuUI.SetActive(true);
        // Ensure options is closed when first pausing
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);

        // Freeze time
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Unlock cursor for menu interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Open the Options panel (and hide the main pause buttons)
    /// </summary>
    public void OpenOptions()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(true);
    }

    /// <summary>
    /// Close Options and return to the main pause buttons
    /// </summary>
    public void CloseOptions()
    {
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
    }

    /// <summary>
    /// Load the Main Menu scene
    /// </summary>
    public void LoadMenu()
    {
        // Always reset time before leaving the scene
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Quit the application
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
