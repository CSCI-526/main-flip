using UnityEngine;

public class M3Trigger : MonoBehaviour
{
    public GameObject M3;
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

        if (M3) M3.SetActive(true);

        Destroy(gameObject);
    }
}

