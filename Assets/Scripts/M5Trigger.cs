using UnityEngine;

public class M5Trigger : MonoBehaviour
{

    [Header("Magnet 5")]
    public GameObject magnet5Active;
    public GameObject magnet5Inactive;

    [Header("Disappear Platform 1")]
    public GameObject platform;
    private bool isTriggered = false;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        if (!other.CompareTag("Player")) return;
        isTriggered = true;

        if (magnet5Active)
        {
            magnet5Active.SetActive(true);
        }
            
        if (magnet5Inactive)
        {
            magnet5Inactive.SetActive(false);
        }

        if (platform)
        {
            platform.SetActive(false);
        }

        Destroy(gameObject);
    }
}
