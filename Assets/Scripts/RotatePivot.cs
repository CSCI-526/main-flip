using UnityEngine;
using System.Collections;

public class RotatePivot : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float angleA = 0f;
    public float angleB = 90f;
    public float rotateTime = 1f;
    public float pauseTime = 1.5f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Start State")]
    public bool startPaused = false;

    [Header("One-shot Mode")]
    public bool rotateOnceOnResume = false;

    private Rigidbody2D rb;
    private Coroutine loopRoutine;
    private bool paused = false;
    private bool returningToStart = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnEnable()
    {
        if (startPaused)
        {
            paused = true;
            returningToStart = false;
            rb.MoveRotation(angleA);
        }
        else
        {
            loopRoutine = StartCoroutine(RunLoop());
        }
    }

    void OnDisable()
    {
        if (loopRoutine != null) StopCoroutine(loopRoutine);
    }

    IEnumerator RunLoop()
    {
        bool toB = true;
        while (true)
        {
            if (paused || returningToStart)
            {
                yield return null;
                continue;
            }

            float target = toB ? angleB : angleA;
            yield return RotateTo(target, rotateTime);
            yield return new WaitForSeconds(pauseTime);
            toB = !toB;
        }
    }

    IEnumerator RotateTo(float targetZ, float duration)
    {
        float startZ = transform.eulerAngles.z;
        float t = 0f;

        while (t < duration)
        {
            if (paused || returningToStart) yield break;
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float e = ease.Evaluate(k);
            float z = Mathf.LerpAngle(startZ, targetZ, e);
            rb.MoveRotation(z);
            yield return null;
        }

        rb.MoveRotation(targetZ);
    }

    public void PauseRotation(bool state)
    {
        if (state && !paused)
        {
            paused = true;
            returningToStart = true;
            StopAllCoroutines();
            StartCoroutine(ReturnToStart());
        }
        else if (!state && paused)
        {
            paused = false;
            returningToStart = false;
            StopAllCoroutines();

            if (rotateOnceOnResume)
            {
                loopRoutine = StartCoroutine(RotateOnceThenPause());
            }
            else
            {
                loopRoutine = StartCoroutine(RunLoop());
            }
        }
    }

    IEnumerator ReturnToStart()
    {
        float currentZ = transform.eulerAngles.z;
        float duration = 0.4f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float e = ease.Evaluate(k);
            float z = Mathf.LerpAngle(currentZ, angleA, e);
            rb.MoveRotation(z);
            yield return null;
        }

        rb.MoveRotation(angleA);
        returningToStart = false;
    }

    IEnumerator RotateOnceThenPause()
    {
        float current = transform.eulerAngles.z;
        float dA = Mathf.Abs(Mathf.DeltaAngle(current, angleA));
        float dB = Mathf.Abs(Mathf.DeltaAngle(current, angleB));
        float target = (dA < dB) ? angleB : angleA;

        yield return RotateTo(target, rotateTime);

        paused = true;
        returningToStart = false;
        loopRoutine = null;
    }

    public bool IsPaused() => paused;
}
