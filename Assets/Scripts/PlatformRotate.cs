using UnityEngine;

public class PlatformRotate : MonoBehaviour
{

    public float rotationSpeed = 10f;
    private Rigidbody2D rb;
    private bool isRotating = false;

    // Initialize the rotating platform
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = false;
    }

    // FixedUpdate is called when triggered, to make the platform rotate.
    void FixedUpdate()
    {
        if (!isRotating) return;
        rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
    }

    public void Activate()
    {
        isRotating = true;
    }

    public void Deactivate()
    {
        isRotating = false;
    }
}
