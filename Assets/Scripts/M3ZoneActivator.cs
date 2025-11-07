using UnityEngine;

public class M3ZoneActivator : MonoBehaviour
{
    public GameObject M3;
        
    private bool zoneLogicArmed = false;
    private bool playerInZone = false;

    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (M3) M3.SetActive(false);
    }

    public void ActivateZoneLogic()
    {
        zoneLogicArmed = true;
        UpdateM3State();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            UpdateM3State();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            UpdateM3State();
        }
    }

    private void UpdateM3State()
    {
        if (!M3) return;
        
        bool shouldBeActive = zoneLogicArmed && playerInZone;
        
        if (M3.activeSelf != shouldBeActive)
        {
            M3.SetActive(shouldBeActive);
        }
    }
}