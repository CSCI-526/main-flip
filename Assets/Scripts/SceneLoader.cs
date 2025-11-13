// using UnityEngine;
// using UnityEngine.SceneManagement; 

// /// <summary>
// /// Handles all scene loading and transitioning logic for the game.
// /// </summary>
// public class SceneLoader : MonoBehaviour
// {
//     // Static instance makes it easy to call this from any other script (like a button click or PlayerController).
//     public static SceneLoader Instance { get; private set; }

//     void Awake()
//     {
//         // Basic Singleton pattern to ensure only one SceneLoader exists.
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject); // Optional: keep scene loader active across scenes
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     /// <summary>
//     /// Loads a scene by its name. Use this for button clicks and level transitions.
//     /// </summary>
//     /// <param name="sceneName">The name of the scene (e.g., "Level_01" or "MainMenu").</param>
//     public void LoadScene(string sceneName)
//     {
//         Debug.Log("Loading Scene: " + sceneName);

//         // --- ANALYTICS INTEGRATION: INITIALIZE SESSION ---
//         // Check if we are loading a gameplay level (assuming your levels start with "Level")
//         if (sceneName.StartsWith("Level")) // Checks for Level, Level_1, Level 2, etc.
//         {
//             // CRITICAL FIX: Initialize the session FIRST before loading the scene.
//             if (AnalyticsManager.Instance != null)
//             {
//                 AnalyticsManager.Instance.InitializeNewSession();
//             }
//         }
//         // -------------------------------------------------
        
//         // Note: The scene must be added to the Build Settings in Unity!
//         SceneManager.LoadScene(sceneName);
//     }
    
//     /// <summary>
//     /// Loads the next level in the build order.
//     /// </summary>
//     public void LoadNextLevel()
//     {
//         int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
//         int nextSceneIndex = currentSceneIndex + 1;
        
//         if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
//         {
//             // The LoadScene function will handle the analytics initialization check
//             // Note: SceneManager.LoadScene(int) internally calls SceneManager.LoadScene(string)
//             // for the new scene, but we should call our wrapper for clarity.
//             LoadScene(SceneUtility.GetScenePathByBuildIndex(nextSceneIndex));
//         }
//         else
//         {
//             Debug.Log("All levels complete! Returning to Main Menu.");
//             // Assuming your Main Menu is the first scene (index 0)
//             LoadScene(SceneUtility.GetScenePathByBuildIndex(0)); 
//         }
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles all scene loading and transitioning logic for the game.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // Static instance makes it easy to call this from any other script (like a button click or PlayerController).
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        // Basic Singleton pattern to ensure only one SceneLoader exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep scene loader active across scenes.
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
        Debug.Log($"[SceneLoader] Loading Scene: {sceneName}");

        // --- OPTIONAL ANALYTICS HOOK ---
        // If your analytics system tracks sessions or starts new level logs:
        /*
        if (sceneName.StartsWith("Level"))
        {
            if (AnalyticsManager.Instance != null)
                AnalyticsManager.Instance.InitializeNewSession();
        }
        */
        // -------------------------------------------------

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Loads the next level based on the Build Index order.
    /// </summary>
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(
                SceneUtility.GetScenePathByBuildIndex(nextSceneIndex)
            );

            Debug.Log($"[SceneLoader] Loading next scene: {nextSceneName}");
            LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("[SceneLoader] All levels complete! Returning to Main Menu.");
            LoadScene("MainMenu");
        }
    }
}
