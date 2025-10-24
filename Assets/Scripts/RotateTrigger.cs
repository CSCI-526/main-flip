using UnityEngine;

public class RotateTrigger : MonoBehaviour
{

    public PlatformRotate platform;
    private bool isTriggered = false;

    // To make sure the collider is a trigger
    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    // When the player triggers, platform starts rotating and the trigger is destroyed.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            platform.Activate();
            Destroy(gameObject);
            LevelAnalytics.Instance?.OnTriggerActivated(); 
        }         
    }
}
