using UnityEngine;

public class TriggerZoneBToC : MonoBehaviour
{
    public GameObject[] toDeactivate;
    public GameObject[] toActivate;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var zone in toDeactivate)
            if (zone) zone.SetActive(false);

        foreach (var zone in toActivate)
            if (zone) zone.SetActive(true);

        Destroy(gameObject);
    }
}
