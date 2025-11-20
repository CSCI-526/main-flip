using UnityEngine;

public class PlayerRespawnControl : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.Instance.RespawnPlayer();
        }
    }
}
