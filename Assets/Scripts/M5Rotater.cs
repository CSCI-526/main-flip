using UnityEngine;
using System.Collections;

public class M5Rotater : MonoBehaviour
{
    [Header("Rotate Settings")]
    public float intervalSeconds = 3f;
    public float stepDegrees = 180f;
    public float rotateDuration = 0.5f;
    public bool activeOnStart = false;

    private Rigidbody2D rb;
    private bool running = false;
    private Coroutine loopCoro;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (activeOnStart) Activate();
    }

    public void Activate()
    {
        if (running) return;
        running = true;
        loopCoro = StartCoroutine(RunLoop());
    }

    IEnumerator RunLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalSeconds);
            yield return RotateBy(stepDegrees, rotateDuration);
        }
    }

    IEnumerator RotateBy(float delta, float duration)
    {
        if (duration <= 0f)
        {
            rb.MoveRotation(rb.rotation + delta);
            yield break;
        }

        float start = rb.rotation;
        float end = start + delta;
        float t = 0f;

        while (t < duration)
        {
            t += Time.fixedDeltaTime;
            float lerped = Mathf.LerpAngle(start, end, Mathf.Clamp01(t / duration));
            rb.MoveRotation(lerped);
            yield return new WaitForFixedUpdate();
        }
        rb.MoveRotation(end);
    }
}
