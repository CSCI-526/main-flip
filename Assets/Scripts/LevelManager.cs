using System.Net.NetworkInformation;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Gate")]
    public Gate gate;

    [Header("Respawn Point")]
    public Transform defaultSpawn;
    private Vector3 currentRespawnPos;
    private bool isRespawning = false;

    [Header("Respawn Freeze")]
    [SerializeField] private float freezeSeconds = 1f;


    // Initialize
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentRespawnPos = defaultSpawn.position;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Set the current active checkpoint.
    public void SetActiveCheckpoint(Vector3 pos)
    {
        currentRespawnPos = pos;
    }

    // Freeze all players for freezeSeconds, then resume
    public void RespawnPlayer()
    {
        if (isRespawning) return;
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        var players = GameObject.FindGameObjectsWithTag("Player");

        // teleport and freeze
        foreach (var p in players)
        {
            if (!p) continue;

            p.transform.position = currentRespawnPos;

            var rb = p.GetComponent<Rigidbody2D>();

            if (rb) rb.SetRotation(0f);                
            p.transform.rotation = Quaternion.identity; 
            var s = p.transform.localScale;              
            p.transform.localScale = new Vector3(s.x >= 0 ? 1f : -1f, 1f, 1f);

            if (rb)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false;                   
            }
    
        }

        yield return new WaitForSeconds(freezeSeconds);

        // unfreeze
        foreach (var p in players)
        {
            if (!p) continue;

            var rb = p.GetComponent<Rigidbody2D>();

            if (rb) rb.simulated = true;
        }

        isRespawning = false;
    }

    

}
