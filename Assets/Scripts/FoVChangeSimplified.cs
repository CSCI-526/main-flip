using UnityEngine;

public class FoVChangeSimplified : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float inAreaSize = 9f;
    public float changeSpeed = 2f;
    private float newSize;
    private float originalSize;

    public bool isChangingFoV = false;
    private LevelManager levelManager;

    void Start()
    {
        originalSize = cam.orthographicSize;
        newSize = originalSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            isChangingFoV = true;
            newSize = inAreaSize;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isChangingFoV = false;
    }

    private void Update()
    {
        if (isChangingFoV)
        {
            if (cam.orthographicSize != newSize)
            {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, Time.deltaTime * changeSpeed);
                if (Mathf.Abs(cam.orthographicSize - newSize) < 0.01f)
                {
                    cam.orthographicSize = newSize;
                    isChangingFoV = false;
                }
            }
        }
    }
        
}
