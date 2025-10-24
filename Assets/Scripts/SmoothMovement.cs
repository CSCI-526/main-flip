using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    public Vector3 targetOffset;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    public bool isActive = false;
    public float speed = 2f;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = transform.position + targetOffset;
    }

    void FixedUpdate()
    {
        if (isActive) {
            if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
        else {
            if (Vector3.Distance(transform.position, initialPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.fixedDeltaTime);
            }
            else
            {
                transform.position = initialPosition;
            }
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
