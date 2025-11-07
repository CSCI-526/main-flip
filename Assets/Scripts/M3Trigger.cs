using UnityEngine;

public class M3Trigger : MonoBehaviour
{
    [Header("Targets")]
    public M3ZoneActivator zoneActivator;
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
                
        if (zoneActivator)
        {
            zoneActivator.ActivateZoneLogic();
        }
        
        Destroy(gameObject);
    }
}