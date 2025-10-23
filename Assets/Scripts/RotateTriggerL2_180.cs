using UnityEngine;
using System.Collections;

public class RotateTriggerL2_180 : MonoBehaviour
{
    public PlatformRotateL2_180 platform;
    [Header("Player Follow Options")]
    public bool carryPlayer = true;
    private bool isTriggered = false;

    private Transform playerOrigParent;
    private Vector3   playerOrigLocalScale;

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

        if (carryPlayer)
        {
            playerOrigParent = player.transform.parent;
            if (rb) rb.freezeRotation = true;
            player.transform.SetParent(platform.transform, true);
        }

        platform.Activate();
        Destroy(gameObject);

        StartCoroutine(RestoreAfter(platform.duration, player, rb));
    }

    IEnumerator RestoreAfter(float wait, GameObject player, Rigidbody2D rb)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, wait) + 0.05f);

        if (carryPlayer && player)
        {
            player.transform.SetParent(playerOrigParent, true);
            if (rb) rb.freezeRotation = false;
        }

        
    }
    
}
