// using UnityEngine;

// public class M2Trigger : MonoBehaviour
// {
//     public GameObject M2;
//     private bool isTriggered = false;

//     void Reset()
//     {
//         var col = GetComponent<Collider2D>();
//         if (col) col.isTrigger = true;
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (isTriggered) return;
//         if (!other.CompareTag("Player")) return;

//         isTriggered = true;

//         if (M2) M2.SetActive(true);

//         Destroy(gameObject);
//     }
// }


using UnityEngine;

public class M2Trigger : MonoBehaviour
{
    public GameObject M2;
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

        // --- PLAY TRIGGER SOUND ---
        other.GetComponent<PlayerAudio>()?.PlayTriggerSound();
        // --------------------------

        if (M2) M2.SetActive(true);

        Destroy(gameObject);
    }
}
