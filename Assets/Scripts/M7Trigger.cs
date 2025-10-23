using UnityEngine;
using System.Collections;

public class M7Trigger : MonoBehaviour
{

    [Header("Move Platform 2")]
    public Transform platform;
    public float targetWorldY = -5.5f;
    public float moveDuration = 1.0f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0,1,1);

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

        if (M7_N) M7_N.SetActive(false);
        if (M7_S) M7_S.SetActive(true);
        
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        if (platform)
            yield return MovePlatformToY(platform, targetWorldY, moveDuration);

        Destroy(gameObject);
    }

    IEnumerator MovePlatformToY(Transform t, float targetY, float duration)
    {
        Vector3 startPos = t.position;
        Vector3 endPos = new Vector3(startPos.x, targetY, startPos.z);

        var rb = t.GetComponent<Rigidbody2D>();
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float k = Mathf.Clamp01(time / duration);
            float e = ease.Evaluate(k);
            Vector3 p = Vector3.LerpUnclamped(startPos, endPos, e);

            if (rb)
                rb.MovePosition(p);
            else
                t.position = p;

            yield return null;
        }

        if (rb) 
            rb.MovePosition(endPos);
        else
            t.position = endPos;
    }
}
