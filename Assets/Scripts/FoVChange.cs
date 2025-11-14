using UnityEngine;

public class FoVChange : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float inAreaSize = 9f;
    public float changeSpeed = 2f;
    private float newSize;
    private float originalSize;

    public bool isChangingFoV = false;
    public bool isChangingForLeaving = false;
    private FoVChange[] foVChanges;
    private LevelManager levelManager;

    void Start()
    {
        originalSize = cam.orthographicSize;
        newSize = originalSize;
        foVChanges = Object.FindObjectsOfType<FoVChange>();
        levelManager = Object.FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            foreach (FoVChange foVChange in foVChanges)
            {
                if (foVChange.gameObject == null) continue;
                foVChange.isChangingFoV = false;
                //Debug.Log("[OnTriggerEnter2D @ " + this.gameObject.name + "] setting isChangingFoV to false on " + foVChange.gameObject.name);
            }
            isChangingFoV = true;
            //Debug.Log("[OnTriggerEnter2D @ " + this.gameObject.name + "] setting isChangingFoV to true");
            newSize = inAreaSize;
            //Debug.Log("[]OnTriggerEnter2D @ " + this.gameObject.name + "] newSize set to " + newSize);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (levelManager.isRespawning)
        {
            return;
        }
        if (other.transform == target)
        {
            isChangingForLeaving = true;
            isChangingFoV = true;
            //Debug.Log("[OnTriggerExit2D @ " + this.gameObject.name + "] setting isChangingFoV to true for leaving");
            newSize = originalSize;
        }
    }

    private void Update()
    {
        if (isChangingFoV)
        {
            if (isChangingForLeaving)
            {
                foreach (FoVChange foVChange in foVChanges)
                {
                    //Debug.Log("[Update @ " + this.gameObject.name + "] Checking FoVChange on " + foVChange.gameObject.name+". isChangingFoV = " + foVChange.isChangingFoV);
                    if (foVChange.isChangingFoV && !foVChange.isChangingForLeaving)
                    {
                        //Debug.Log("[Update @ " + this.gameObject.name + "] Another FoVChange (" + foVChange.gameObject.name + ") is still changing FoV. Canceling leave change.");
                        isChangingFoV = false;
                        isChangingForLeaving = false;
                        return;
                    }
                }
            }

            if (cam.orthographicSize != newSize)
            {
                //Debug.Log("[Update @ " + this.gameObject.name + "] Changing FoV from " + cam.orthographicSize + " to " + newSize);
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
