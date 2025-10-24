using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
    public GameObject platform;
    public SmoothMovement smoothMovement;
    private bool isTriggered = false;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            smoothMovement.Activate();
            Destroy(gameObject);
        }
        
    }
}
