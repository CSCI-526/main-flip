using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Respawn Point")]
    public Transform respawnPoint;

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;
        if (!other.CompareTag("Player")) return;

        isActivated = true;

        Vector3 pos = respawnPoint ? respawnPoint.position : transform.position;

        LevelManager.Instance.SetActiveCheckpoint(pos);
    }
}

