using UnityEngine;

public class Hazard : MonoBehaviour
{

    // When the player collides with the hazard, respawn at checkpoint.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        LevelManager.Instance.RespawnPlayer();
    }
}
