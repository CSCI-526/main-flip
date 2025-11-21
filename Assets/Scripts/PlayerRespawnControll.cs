using UnityEngine;

public class PlayerRespawnControl : MonoBehaviour
{
    private InputManager inputManager;

    void Start()
    {
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        if (inputManager != null && Input.GetKeyDown(inputManager.keyMappings["Respawn"]))
        {
            LevelManager.Instance.RespawnPlayer();
        }
    }
}
