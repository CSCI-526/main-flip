using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Respawn Point")]
    public Transform respawnPoint;

    [Tooltip("1=C1, 2=C2, 3=C3, 4=C4")]
    public int checkpointIndex = 1;

    public string checkpointId = "A1";
    public int idealOpsToNext = 0;

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;
        if (!other.CompareTag("Player")) return;

        isActivated = true;

        Vector3 pos = respawnPoint ? respawnPoint.position : transform.position;

        LevelManager.Instance.SetActiveCheckpoint(pos);
        Debug.Log("[Checkpoint] Player hit checkpoint " + checkpointId);
        OperationAnalytics.Instance?.OnCheckpointReached(this);

        Destroy(gameObject);
    }
}

