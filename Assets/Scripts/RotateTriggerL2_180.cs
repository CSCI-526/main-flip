using UnityEngine;
using System.Collections;

public class RotateTriggerL2_180 : MonoBehaviour
{
    public PlatformRotateL2_180 pivotScript; 

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
        if (!other.CompareTag("Player")) return;

        var player = other.gameObject;
        var rb = player.GetComponent<Rigidbody2D>();

        isTriggered = true;

        if (pivotScript != null)
        {
            pivotScript.TriggerRotateOnce();
        }

        Destroy(gameObject);

    }
    
}
