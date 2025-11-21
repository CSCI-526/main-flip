using UnityEngine;

public class TriggerZoneAToB : MonoBehaviour
{
    public RotatePivot PivottoStart;
    public GameObject[] toDeactivate;
    public GameObject[] toActivate;
    public GameObject[] toDestroy;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PivottoStart) 
            PivottoStart.PauseRotation(false);

        foreach (var zone in toDeactivate)
            if (zone) zone.SetActive(false);

        foreach (var zone in toActivate)
            if (zone) zone.SetActive(true);

        foreach (var obj in toDestroy)

            if (obj) Destroy(obj);

        Destroy(gameObject);
    }
}
