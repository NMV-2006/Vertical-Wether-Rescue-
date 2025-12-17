using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main Menu Controller
/// Handles main menu interactions including starting the game, opening options, and quitting.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name of the scene to load when Start is pressed.")]
    public string firstLevelSceneName = "Level1"; 

    [Header("UI Panels")]
    [Tooltip("The main menu panel containing Start, Options, Exit buttons.")]
    public GameObject mainMenuPanel;

    [Tooltip("The options panel to display when Options is pressed.")]
    public GameObject optionsPanel;

    private void Start()
    {
        // Ensure starting state: Main Menu ON, Options OFF
        ReturnToMainMenu();
    }

    /// <summary>
    /// Called by the Start Button
    /// </summary>
    public void StartGame()
    {
        Debug.Log($"[MainMenu] Starting game... Loading scene: {firstLevelSceneName}");
        
        // Check if the scene is in Build Settings (common error)
        if (Application.CanStreamedLevelBeLoaded(firstLevelSceneName))
        {
            SceneManager.LoadScene(firstLevelSceneName);
        }
        else
        {
            Debug.LogError($"[MainMenu] Scene '{firstLevelSceneName}' not found! \n1. Go to File -> Build Settings\n2. Add the scene to the list\n3. Check the spelling in the Inspector.");
        }
    }

    /// <summary>
    /// Called by the Options Button
    /// </summary>
    public void OpenOptions()
    {
        Debug.Log("[MainMenu] Opening Options...");

        if (optionsPanel == null)
        {
            Debug.LogError("CRITICAL ERROR: 'Options Panel' is NOT assigned in the Inspector! Select your MainMenuManager object and drag the OptionsPanel into the script slot.");
            return;
        }

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    /// <summary>
    /// Called by the Back Button in the Options panel
    /// </summary>
    public void ReturnToMainMenu()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Called by the Exit Button
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("[MainMenu] Quitting Game...");
        
        Application.Quit();

        // If running in Unity Editor, stop playing mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
