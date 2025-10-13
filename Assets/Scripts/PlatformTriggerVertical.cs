using UnityEngine;

//Trigger the platform to move up & down
public class PlatformTriggerVertical : MonoBehaviour
{
    public PlatformMoverVertical platform;
    public float downDistance = 5f;  
    public GameObject hazardToDestroy;
    public GameObject portalToActivate;

    bool used;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;

        used = true;

        if (platform)
            platform.MoveDownOnce(downDistance);

        if (hazardToDestroy)
            Destroy(hazardToDestroy);

        if (portalToActivate)
            portalToActivate.SetActive(true);

        var collider = GetComponent<Collider2D>();
        if (collider)
            collider.enabled = false;

        Destroy(gameObject);
    }
}
