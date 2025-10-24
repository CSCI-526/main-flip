using UnityEngine;

public class Friction : MonoBehaviour
{
    public float frictionStrength = 0.5f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D otherRb = collision.rigidbody;
        if (otherRb != null) {
            if (otherRb.linearVelocity.sqrMagnitude > 0.01f) {
                Vector2 frictionDirection = -otherRb.linearVelocity.normalized;
                float frictionMagnitude = frictionStrength;

                otherRb.AddForce(frictionDirection * frictionMagnitude, ForceMode2D.Force);

                if (otherRb.linearVelocity.magnitude < 0.1f)
                {
                    otherRb.linearVelocity = Vector2.zero;
                }
            }
        }
        

    }
}