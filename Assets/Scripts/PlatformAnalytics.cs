using UnityEngine;

public class InvisibleGoalTrigger : MonoBehaviour
{
    private void Reset()
    {
        var col = GetComponent<BoxCollider2D>();
        if (!col) col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelAnalytics.Instance?.MarkPlatformReached();
        }
    }
}
