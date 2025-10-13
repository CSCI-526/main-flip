using UnityEngine;

//Toggle the platform to move up & down
public class PlatformToggle : MonoBehaviour
{
    public PlatformMoverToggle platform;       
    public float reactivateTime = 0.15f;       

    bool activated = true;

    public GameObject hazardToDestroy;
    public GameObject portalToActivate;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated) return;
        if (!other.CompareTag("Player")) return;

        platform?.ToggleUpDown();

        if (hazardToDestroy)
            Destroy(hazardToDestroy);

        if (portalToActivate)
            portalToActivate.SetActive(true);

        activated = false; 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        activated = true;
    }

}
