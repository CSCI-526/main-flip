using UnityEngine;

public class RotateStopper : MonoBehaviour
{
    public RotatePivot[] toStop;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        foreach (var r in toStop)
            if (r) r.PauseRotation(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var r in toStop)
                if (r) r.PauseRotation(false);
        }
    }
}
