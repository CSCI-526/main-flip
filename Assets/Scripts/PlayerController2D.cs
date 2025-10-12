using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 8f;

    [Header("Gravity")]
    public float gravityMagnitude = 3f;  

    public Rigidbody2D rb;

    float inputX = 0f;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = gravityMagnitude;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        inputX = 0f;
        if (Input.GetKey(KeyCode.A)) inputX = -1f;
        if (Input.GetKey(KeyCode.D)) inputX = 1f;

        if (Input.GetKey(KeyCode.W))
            SetGravity(-gravityMagnitude);   
        else if (Input.GetKey(KeyCode.S))
            SetGravity(+gravityMagnitude);   
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

        if (inputX != 0f)
        {
            var s = transform.localScale;
            transform.localScale = new Vector3(Mathf.Sign(inputX) * Mathf.Abs(s.x), s.y, s.z);
        }
    }

    void SetGravity(float g)
    {
        if (Mathf.Approximately(rb.gravityScale, g)) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
        rb.gravityScale = g;
    }
}
