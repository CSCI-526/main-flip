using UnityEngine;
using System.Collections;

// Toggle the platform moving up & down
public class PlatformMoverToggle : MonoBehaviour
{
    public float downDistance = 5f;
    public float moveTime = 1.0f; 

    Vector3 startPos;
    Vector3 endPos;            
    bool isMoving;
    bool atTop;                   

    Rigidbody2D rb;

    void Awake()
    {
        startPos = transform.position;
        endPos = startPos - new Vector3(0f, Mathf.Abs(downDistance), 0f);
        rb = GetComponent<Rigidbody2D>(); 
    }

    public void ToggleUpDown()
    {
        if (isMoving) return;
        StartCoroutine(Moving(atTop ? startPos : endPos));
        atTop = !atTop;
    }

    IEnumerator Moving(Vector3 target)
    {
        isMoving = true;
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
        isMoving = false;
    }
}
