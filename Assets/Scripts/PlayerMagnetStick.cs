using UnityEngine;

public class PlayerMagnetStick : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject currentPlatform;
    private Vector3 lastPlatformPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (currentPlatform != null)
        {
            Vector3 delta = currentPlatform.transform.position - lastPlatformPos;
            rb.position += (Vector2)delta; 
            lastPlatformPos = currentPlatform.transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Magnetism>() != null)
        {
            currentPlatform = collision.gameObject;
            lastPlatformPos = currentPlatform.transform.position;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == currentPlatform)
        {
            currentPlatform = null;
        }
    }
}
