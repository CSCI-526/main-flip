using UnityEngine;

public class Gate : MonoBehaviour
{

    [Header("Colliders")]
    public BoxCollider2D triggerZone;

    [Header("Gate States")]
    public GameObject openedGate;
    public GameObject levelCompleteUI;

    bool done;


    // Initialize references to colliders and gate states.
    private void Reset()
    {
        triggerZone = transform.Find("TriggerZone")?.GetComponent<BoxCollider2D>();
        openedGate = transform.Find("Opened")?.gameObject;

        if (triggerZone) triggerZone.isTrigger = true;
    }

    // Level complete when player enters the open gate.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (done) return;
        if (!other.CompareTag("Player")) return;

        done = true;

        if (levelCompleteUI)
            levelCompleteUI.SetActive(true);
    }
    
}
