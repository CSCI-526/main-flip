using UnityEngine;

public class FoVFocus : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float moveSpeed = 2f;
    private Vector3 originalPosition;
    private Vector3 newPosition;
    private bool isFocused = false;

    void Start()
    {
        originalPosition = cam.transform.position;
        newPosition = this.transform.position;
        newPosition.z = originalPosition.z;
    }

    private void Update()
    {
        if (isFocused)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, newPosition, moveSpeed * Time.deltaTime);       
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            isFocused = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == target)
        {
            isFocused = false;
        }
    }    
}
