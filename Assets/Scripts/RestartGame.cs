using UnityEngine;
using UnityEngine.SceneManagement; // You need this line to access SceneManager

public class RestartGame : MonoBehaviour
{
    // This is the function the button will call
    public void RestartLevel()
    {
        // 1. Unfreeze Time: Ensures the game starts running again if it was paused (Time.timeScale = 0).
        Time.timeScale = 1f; 

        // 2. Get the name of the current active scene.
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // 3. Load the scene again, effectively restarting the level.
        SceneManager.LoadScene(currentSceneName);
    }
}