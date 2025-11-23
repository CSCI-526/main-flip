using UnityEngine;

public class M5Trigger : MonoBehaviour
{
    [Header("Magnet 5")]
    public GameObject magnet5Active;
    public GameObject magnet5Inactive;

    [Header("Moved Platform 1")]
    public GameObject platform;

    [Header("Move Settings")]
    public float moveLeftDistance = 3f;
    public float moveDuration = 1.0f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

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

        // Play trigger sound
        PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
        if (playerAudio != null)
        {
            playerAudio.PlayTriggerSound();
        }

        if (magnet5Active)   magnet5Active.SetActive(true);
        if (magnet5Inactive) magnet5Inactive.SetActive(false);

        if (platform)
        {
            StripPhysics(platform);

            var t = platform.transform;
            var mover = t.GetComponent<PlatformLeftMove>();
            if (!mover) mover = t.gameObject.AddComponent<PlatformLeftMove>();

            mover.MoveByWorld(new Vector3(-Mathf.Abs(moveLeftDistance), 0f, 0f), moveDuration, ease);
        }

        LevelAnalytics.Instance?.MarkTrigger("Trigger2");

        Destroy(gameObject);
    }


    void StripPhysics(GameObject go)
    {
        foreach (var c in go.GetComponentsInChildren<Collider2D>(true))
            Destroy(c);
        var rb = go.GetComponent<Rigidbody2D>();
        if (rb) Destroy(rb);
    }
}
