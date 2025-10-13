using UnityEngine;
using System.Collections;

//Move the platform vertically
public class PlatformMoverVertical : MonoBehaviour
{
    public float moveTime = 1.0f;

    Rigidbody2D rb;
    bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();     
    }

    public void MoveByOnce(Vector2 delta)
    {
        if (isMoving) return;
        StartCoroutine(Moving(delta));
    }

    public void MoveDownOnce(float distance)
    {
        MoveByOnce(new Vector2(0f, -Mathf.Abs(distance))); 
    }

    IEnumerator Moving(Vector2 delta)
    {
        isMoving = true;
        Vector3 from = transform.position;
        Vector3 to = from + (Vector3)delta;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, moveTime);
            Vector3 p = Vector3.Lerp(from, to, t); 
            if (rb) rb.MovePosition(p); else transform.position = p;
            yield return null;
        }
        isMoving = false;
    }
}
