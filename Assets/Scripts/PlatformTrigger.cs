using UnityEngine;

//Trigger the platform to move left & right
public class PlatformTrigger : MonoBehaviour
{
    public PlatformMover platform;
    public GameObject hazardToDestroy;
    public GameObject portalToActivate;

    bool used;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;

        used = true;

        if (hazardToDestroy)
            Destroy(hazardToDestroy);

        if (portalToActivate)
            portalToActivate.SetActive(true);

        if (platform)
            platform.StartOscillating();

        var collider = GetComponent<Collider2D>();
        if (collider)
            collider.enabled = false;

        Destroy(gameObject);
    }
}
