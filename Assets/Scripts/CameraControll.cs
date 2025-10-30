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
        if (target == null)
        {
            return;
        }


        Vector3 targetPosition = target.position;
        Vector3 currentCamPosition = transform.position;
        Vector3 targetCamPosition = currentCamPosition;
        Vector3 viewportPoint = cam.WorldToViewportPoint(targetPosition);
        float zDistance = Mathf.Abs(currentCamPosition.z - targetPosition.z);

        if (viewportPoint.x < minX)
        {
            float edgeWorldX = cam.ViewportToWorldPoint(new Vector3(minX, 0.5f, zDistance)).x;
            float deltaX = targetPosition.x - edgeWorldX;
            targetCamPosition.x = currentCamPosition.x + deltaX;
        }
        else if (viewportPoint.x > maxX)
        {
            float edgeWorldX = cam.ViewportToWorldPoint(new Vector3(maxX, 0.5f, zDistance)).x;
            float deltaX = targetPosition.x - edgeWorldX;
            targetCamPosition.x = currentCamPosition.x + deltaX;
        }

        if (viewportPoint.y < minY)
        {
            float edgeWorldY = cam.ViewportToWorldPoint(new Vector3(0.5f, minY, zDistance)).y;
            float deltaY = targetPosition.y - edgeWorldY;
            targetCamPosition.y = currentCamPosition.y + deltaY;
        }
        else if (viewportPoint.y > maxY)
        {
            float edgeWorldY = cam.ViewportToWorldPoint(new Vector3(0.5f, maxY, zDistance)).y;
            float deltaY = targetPosition.y - edgeWorldY;
            targetCamPosition.y = currentCamPosition.y + deltaY;
        }


        targetCamPosition.z = currentCamPosition.z;
        Vector3 smoothedPosition = Vector3.Lerp(currentCamPosition, targetCamPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}