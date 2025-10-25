using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading

/// <summary>
/// Handles all scene loading and transitioning logic for the game.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // Static instance makes it easy to call this from any other script (like a button click or PlayerController).
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        // Basic Singleton pattern to ensure only one SceneLoader exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keep scene loader active across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads a scene by its name. Use this for button clicks and level transitions.
    /// </summary>
    /// <param name="sceneName">The name of the scene (e.g., "Level_01" or "MainMenu").</param>
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName);
        // Note: The scene must be added to the Build Settings in Unity!
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// Loads the next level in the build order.
    /// </summary>
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // Check if we've run out of levels (e.g., go back to the Main Menu)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("All levels complete! Returning to Main Menu.");
            // Assuming your Main Menu is the first scene (index 0)
            SceneManager.LoadScene(0); 
        }
    }
}

