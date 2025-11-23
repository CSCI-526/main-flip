using UnityEngine;
using System.Collections;

public class M5Rotater : MonoBehaviour
{
    [Header("Rotate Settings")]
    public float intervalSeconds = 3f;
    public float stepDegrees = 180f;
    public float rotateDuration = 0.5f;
    public bool activeOnStart = false;
    
    [Header("Rotation Direction")]
    public bool startClockwise = true;
    
    private Rigidbody2D rb;
    private bool running = false;
    private Coroutine loopCoro;
    private bool currentClockwise;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentClockwise = startClockwise;
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
            
            float delta = currentClockwise ? stepDegrees : -stepDegrees;
            yield return RotateBy(delta, rotateDuration);
            
            currentClockwise = !currentClockwise;
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
            float ratio = Mathf.Clamp01(t / duration);
            
            float current = start + delta * ratio;
            
            rb.MoveRotation(current);
            yield return new WaitForFixedUpdate();
        }

        rb.MoveRotation(end);
    }
}