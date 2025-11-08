using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
    public GameObject platform;
    public SmoothMovement smoothMovement;
    private bool isTriggered = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
            if (spriteRenderer) spriteRenderer.enabled = false;

            // isTriggered = true;
            smoothMovement.Activate();
            // Destroy(gameObject);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spriteRenderer) spriteRenderer.enabled = true;
        }
    }
}
