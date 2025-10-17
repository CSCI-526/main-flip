
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPoleControll : MonoBehaviour
{
    [Tooltip("The Y coordinate below which the player is considered dead.")]
    public float deathYPosition = -10f;

    [Tooltip("The UI Panel to show when the player dies (must be disabled initially).")]
    public GameObject deathUI;

    void Update()
    {
        CheckForDeath();
        UpdatePole();
    }

    void UpdatePole()
    {
        Magnetism magetism = GetComponent<Magnetism>(); 

        if (magetism == null) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            magetism.currentPole = MagneticPole.South;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            magetism.currentPole = MagneticPole.North;
        }
    }

    void CheckForDeath()
    {
        if (transform.position.y < deathYPosition)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathUI != null && deathUI.activeSelf) return;

        Debug.Log("Player Died: Fallen out of world bounds.");

        Time.timeScale = 0f;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true; 
        }

        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }
    }

        public void RestartGame()
    {
       
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
