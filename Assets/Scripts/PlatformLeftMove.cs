using UnityEngine;
using System.Collections;

public class PlatformLeftMove : MonoBehaviour
{
public float defaultDuration = 1.0f;
    public AnimationCurve defaultEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Rigidbody2D rb;
    private Coroutine moveCo;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic)
                rb.bodyType = RigidbodyType2D.Kinematic;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    public void MoveByWorld(Vector3 delta, float duration = -1f, AnimationCurve ease = null)
    {
        if (duration <= 0f) duration = defaultDuration;
        if (ease == null) ease = defaultEase;

        Vector3 endPos = transform.position + delta;
        if (moveCo != null) StopCoroutine(moveCo);
        moveCo = StartCoroutine(MoveRoutine(endPos, duration, ease));
    }

    IEnumerator MoveRoutine(Vector3 endPos, float duration, AnimationCurve ease)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float e = ease.Evaluate(t);
            Vector3 p = Vector3.LerpUnclamped(startPos, endPos, e);

            if (rb) rb.MovePosition(p);
            else    transform.position = p;

            yield return new WaitForFixedUpdate();
        }

        if (rb) rb.MovePosition(endPos);
        else    transform.position = endPos;

        moveCo = null;
    }
}
