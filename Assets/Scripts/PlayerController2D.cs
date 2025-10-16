using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 8f;

    public Rigidbody2D rb;
    [Header("Compat (read-only for other scripts)")]
    public float gravityMagnitude = 3f;

    float inputX = 0f;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        inputX = 0f; 
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
    }
}
