using UnityEngine;

public class PlayerRespawnControl : MonoBehaviour
{
    private InputManager inputManager;
    private KeyBindUI keyBindUI;

    void Start()
    {
        inputManager = InputManager.Instance;
        keyBindUI = FindObjectOfType<KeyBindUI>(true);
    }

    void Update()
    {
        if (keyBindUI != null && keyBindUI.isRebinding)
            return;

        if (inputManager != null && Input.GetKeyDown(inputManager.keyMappings["Respawn"]))
        {
            Debug.Log("Respawn");
            LevelManager.Instance.RespawnPlayer();
        }
    }
}
