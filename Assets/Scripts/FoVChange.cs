using UnityEngine;

public class FoVChange : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float inAreaSize = 9f;
    public float changeSpeed = 2f;
    private float newSize;
    private float originalSize;

    void Start()
    {
        originalSize = cam.orthographicSize;
        newSize = originalSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            newSize = inAreaSize;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == target)
        {
            newSize = originalSize;
        }
    }

    private void Update()
    {
        if (cam.orthographicSize != newSize)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, Time.deltaTime * changeSpeed);
            if (Mathf.Abs(cam.orthographicSize - newSize) < 0.01f)
            {
                cam.orthographicSize = newSize;
            }
        }
    }
        
}
