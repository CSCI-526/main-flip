using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Gravity")]
    public float gravityMagnitude = 3f;

    public Rigidbody2D rb;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = -Mathf.Abs(gravityMagnitude); 
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            SetGravity(-Mathf.Abs(gravityMagnitude));   
        else if (Input.GetKey(KeyCode.S))
            SetGravity(+Mathf.Abs(gravityMagnitude));   
    }

    void FixedUpdate() { }

    void SetGravity(float g)
    {
        if (Mathf.Approximately(rb.gravityScale, g)) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.gravityScale = g;
    }
}
