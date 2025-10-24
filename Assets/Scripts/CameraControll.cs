using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public float deadZoneX = 0.2f;
    public float deadZoneY = 0.2f;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        minX = 0.5f - deadZoneX;
        maxX = 0.5f + deadZoneX;
        minY = 0.5f - deadZoneY;
        maxY = 0.5f + deadZoneY;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = target.position;
        Vector3 viewportPoint = cam.WorldToViewportPoint(targetPosition);
        Vector3 currentCamPosition = transform.position;
        Vector3 newCamPosition = currentCamPosition;

        if (viewportPoint.x < minX)
        {
            float offset = cam.ViewportToWorldPoint(new Vector3(minX, 0, transform.position.z)).x;
            newCamPosition.x = offset;
        }
        else if (viewportPoint.x > maxX)
        {
            float offset = cam.ViewportToWorldPoint(new Vector3(maxX, 0, transform.position.z)).x;
            newCamPosition.x = offset;
        }

        if (viewportPoint.y < minY)
        {
            float offset = cam.ViewportToWorldPoint(new Vector3(0, minY, transform.position.z)).y;
            newCamPosition.y = offset;
        }
        else if (viewportPoint.y > maxY)
        {
            float offset = cam.ViewportToWorldPoint(new Vector3(0, maxY, transform.position.z)).y;
            newCamPosition.y = offset;
        }

        newCamPosition.z = currentCamPosition.z;
        Vector3 smoothedPosition = Vector3.Lerp(currentCamPosition, newCamPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    //void OnDrawGizmosSelected()
    //{
    //    if (Application.isPlaying)
    //    {
    //        Gizmos.color = Color.cyan;
    //        Gizmos.DrawWireCube(transform.position, new Vector3(deadZoneX * 2, deadZoneY * 2, 0));
    //    }
    //}
}