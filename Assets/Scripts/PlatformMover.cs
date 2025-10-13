using UnityEngine;
using System.Collections;

// Move platform right & left horizontally
public class PlatformMover : MonoBehaviour
{
    public float moveDistance = 9f;
    public float moveTime = 2.5f;
    public float waitAtEnds = 0.5f;

    Vector3 startPos;
    Rigidbody2D rb;
    Coroutine loop;

    void Awake()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartOscillating()
    {
        if (loop == null) loop = StartCoroutine(MoveLoop());
    }

    public void StopOscillating()
    {
        if (loop != null)
        {
            StopCoroutine(loop);
            loop = null;
        }
    }

    IEnumerator MoveLoop()
    {
        Vector3 rightPos = startPos + new Vector3(moveDistance, 0f, 0f);

        while (true)
        {
            yield return MoveTo(rightPos);
            if (waitAtEnds > 0f) yield return new WaitForSeconds(waitAtEnds);

            yield return MoveTo(startPos);
            if (waitAtEnds > 0f) yield return new WaitForSeconds(waitAtEnds);
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        Vector3 from = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, moveTime);
            Vector3 p = Vector3.Lerp(from, target, t);

            if (rb)
            {
                rb.MovePosition(p);
            }
            else
            {
                transform.position = p;
            }

            yield return null;
        }
    }
}
