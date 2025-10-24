using UnityEngine;
using System.Collections;

public class PlatformRotateL2_180 : MonoBehaviour
{

    [Header("Rotation Settings")]
    public float targetAngle = 180f;
    public float duration = 1.0f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0,1,1);

    [Header("Physics Settings")]
    public bool disableChildRigidbodiesDuringRotate = true;

    private Rigidbody2D rb;
    private bool hasRotated = false;
    private bool isRotating = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    public void TriggerRotateOnce()
    {
        if (hasRotated || isRotating) return;
        StartCoroutine(RotateRoutine());
    }

    IEnumerator RotateRoutine()
    {
        isRotating = true;

        Rigidbody2D[] childRBs = null;
        if (disableChildRigidbodiesDuringRotate)
        {
            childRBs = GetComponentsInChildren<Rigidbody2D>();
            foreach (var r in childRBs)
            {
                if (!r || r == rb) continue;
                r.simulated = false;
            }
        }

        float from = rb.rotation;
        float to = from + targetAngle;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float e = ease.Evaluate(k);
            float z = Mathf.LerpAngle(from, to, e);
            rb.MoveRotation(z);
            yield return null;
        }

        rb.MoveRotation(to);

        if (childRBs != null)
        {
            foreach (var r in childRBs)
            {
                if (!r || r == rb) continue;
                r.simulated = true;
            }
        }

        hasRotated = true;
        isRotating = false;
    }

}

