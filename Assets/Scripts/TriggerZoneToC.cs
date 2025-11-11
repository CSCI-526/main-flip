using UnityEngine;

public class TriggerZoneToC : MonoBehaviour
{
    public RotatePivot PivottoStart;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PivottoStart) PivottoStart.PauseRotation(false);

        Destroy (gameObject);
    }
}
