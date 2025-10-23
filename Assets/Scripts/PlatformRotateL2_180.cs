using UnityEngine;
using System.Collections;

public class PlatformRotateL2_180 : MonoBehaviour
{


    [Header("Pivot")]
    public Transform pivotTransform;
    
    [Header("Rotation")]
    public float targetAngle = 180f;
    public float duration = 0.5f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Physics")]
    public bool disableRigidbodiesDuringRotate = true;

    bool hasRotated = false;
    bool isRotating = false;


    public void Activate()
    {
        if (hasRotated || isRotating) return;
        StartCoroutine(RotateRoutine());
    }

    IEnumerator RotateRoutine()
    {
        isRotating = true;

        Rigidbody2D[] rb2ds = disableRigidbodiesDuringRotate ? GetComponentsInChildren<Rigidbody2D>() : null;
        if (rb2ds != null)
        {
            foreach (var rb in rb2ds) if (rb) rb.simulated = false;
        }

        Vector3 pivot = pivotTransform.position;

        float elapsed = 0f;
        float prev = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = ease.Evaluate(t);
            float current = Mathf.LerpUnclamped(0f, targetAngle, eased);
            float delta = current - prev;
            prev = current;

            transform.RotateAround(pivot, Vector3.forward, delta);
            yield return null;
        }

        if (rb2ds != null)
        {
            foreach (var rb in rb2ds) if (rb) rb.simulated = true;
        }

        hasRotated = true;
        isRotating = false;
    }


}

