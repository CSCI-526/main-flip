using UnityEngine;
using System.Collections;

public class PlatformDescend : MonoBehaviour
{

    [Header("Default Move Settings (can be overridden)")]
    public float defaultDuration = 1.0f;
    public AnimationCurve defaultEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Rigidbody2D rb;
    private Coroutine moveCo;
    private bool moving;

    public void MoveToWorldY(float targetWorldY, float duration = -1f, AnimationCurve ease = null)
    {
        Vector3 start = transform.position;
        Vector3 end = new Vector3(start.x, targetWorldY, start.z);
        MoveToWorldPosition(end, duration, ease);
    }

    public void MoveToWorldPosition(Vector3 worldPos, float duration = -1f, AnimationCurve ease = null)
    {
        if (duration <= 0f) duration = defaultDuration;
        if (ease == null) ease = defaultEase;

        if (moveCo != null) StopCoroutine(moveCo);
        moveCo = StartCoroutine(MoveRoutine(worldPos, duration, ease));
    }

    IEnumerator MoveRoutine(Vector3 endPos, float duration, AnimationCurve ease)
    {
        moving = true;

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float e = ease.Evaluate(t);
            Vector3 p = Vector3.LerpUnclamped(startPos, endPos, e);

            if (rb) rb.MovePosition(p);
            else transform.position = p;

            yield return new WaitForFixedUpdate();
        }

        if (rb) rb.MovePosition(endPos);
        else transform.position = endPos;

        moving = false;
        moveCo = null;
    }
    
}
