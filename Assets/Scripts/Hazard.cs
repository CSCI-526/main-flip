using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        string levelName = SceneManager.GetActiveScene().name;
        Vector2 pos = collision.transform.position;

        SendAnalytics.SendDeath(levelName, pos.x, pos.y);

        LevelManager.Instance.RespawnPlayer();
    }
}
