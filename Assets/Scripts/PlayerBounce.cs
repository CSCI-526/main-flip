using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public float bounceForce = 12f; 

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.contactCount == 0) return;
        Vector2 n = c.GetContact(0).normal;
        rb.AddForce(-n * bounceForce, ForceMode2D.Impulse);
    }
}
