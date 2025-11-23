using UnityEngine;

public class M7Trigger : MonoBehaviour
{
    [Header("Target Platform")]
    public Transform platform;
    public float targetWorldY = -5.5f;
    public float moveDuration = 1.0f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Magnet 7")]
    public GameObject M7_N;
    public GameObject M7_S;

    bool isTriggered = false;

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

        // Play trigger sound
        PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
        if (playerAudio != null)
        {
            playerAudio.PlayTriggerSound();
        }

        if (M7_N) M7_N.SetActive(false);
        if (M7_S) M7_S.SetActive(true);

        if (platform)
        {
            var mover = platform.GetComponent<PlatformDescend>();
            if (!mover) mover = platform.gameObject.AddComponent<PlatformDescend>();
            mover.MoveToWorldY(targetWorldY, moveDuration, ease);
        }

        Destroy(gameObject);
    }

}
