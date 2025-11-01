using UnityEngine;

public class M5RotateTrigger : MonoBehaviour
{
    public M5Rotater rotator;

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

        if (rotator) rotator.Activate();

        Destroy(gameObject);
    }
}
