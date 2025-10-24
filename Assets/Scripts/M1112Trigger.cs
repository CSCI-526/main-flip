using UnityEngine;

public class M1112Trigger : MonoBehaviour
{
    public RotatePivot[] toStart;
    public GameObject M11_N;
    public GameObject M11_S;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var r in toStart)
            if (r) r.PauseRotation(false);

        if (M11_N) M11_N.SetActive(false);
        if (M11_S) M11_S.SetActive(true);

        Destroy(gameObject);
    }
}
